namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Collections =
    let AsyncPageCollection token user folderid limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/collections/{Utils.guidString folderid}"
        |> Utils.asyncRead
        |> Utils.thenParse<FolderPage>

    let GetCollectionAsync token user folderid batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageCollection token user folderid batchsize offset)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let AsyncPageAll token user limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/collections/all"
        |> Utils.asyncRead
        |> Utils.thenParse<FolderPage>

    let AsyncGetAll token user batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageAll token user batchsize offset)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let AsyncPageFolders token calculateSize extPreload filterEmptyFolder user limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.calculateSize calculateSize
            yield! QueryFor.folderPreload extPreload
            yield! QueryFor.filterEmptyFolder filterEmptyFolder
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Utils.asyncRead
        |> Utils.thenParse<Page<CollectionFolder>>

    let GetFoldersAsync token calculateSize extPreload filterEmptyFolder user batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageFolders token calculateSize extPreload filterEmptyFolder user batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let AsyncCopyDeviations token target_folderid deviationids =
        seq {
            "target_folderid", Utils.guidString target_folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/copy_deviations"
        |> Utils.asyncRead
        |> Utils.thenParse<SuccessOrErrorResponse>

    let AsyncMoveDeviations token source_folderid target_folderid deviationids =
        seq {
            "source_folderid", Utils.guidString source_folderid
            "target_folderid", Utils.guidString target_folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/move_deviations"
        |> Utils.asyncRead
        |> Utils.thenParse<SuccessOrErrorResponse>

    let AsyncRemoveDeviations token folderid deviationids =
        seq {
            "folderid", Utils.guidString folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/move_deviations"
        |> Utils.asyncRead
        |> Utils.thenParse<SuccessOrErrorResponse>

    type FaveResult = {
        success: bool
        favourites: int
    }

    let AsyncFave token deviationid folderids =
        seq {
            yield "deviationid", Utils.guidString deviationid
            let mutable index = 0
            for f in folderids do
                yield $"folderid[{index}]", Utils.guidString f
                index <- index + 1
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        |> Utils.asyncRead
        |> Utils.thenParse<FaveResult>

    let AsyncUnfave token deviationid folderids =
        seq {
            yield "deviationid", Utils.guidString deviationid
            let mutable index = 0
            for f in folderids do
                yield $"folderid[{index}]", Utils.guidString f
                index <- index + 1
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/unfave"
        |> Utils.asyncRead
        |> Utils.thenParse<FaveResult>

    let AsyncCreateFolder token folder =
        seq {
            yield "folder", folder
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create"
        |> Utils.asyncRead
        |> Utils.thenParse<CollectionFolder>

    let AsyncRemoveFolder token folderid =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/{Utils.guidString folderid}"
        |> Utils.asyncRead
        |> Utils.thenParse<SuccessOrErrorResponse>

    let AsyncUpdateFolder token folderid folderUpdates =
        seq {
            yield "folderid", Utils.guidString folderid
            yield! QueryFor.folderUpdates folderUpdates
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/update"
        |> Utils.asyncRead
        |> Utils.thenParse<SuccessOrErrorResponse>

    let AsyncUpdateDeviationOrder token folderid deviationid position =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "deviationid", deviationid
            yield "position", Utils.intString position
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/update_deviation_order"
        |> Utils.asyncRead
        |> Utils.thenMap ignore

    let AsyncUpdateOrder token folderid position =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "position", Utils.intString position
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/update_order"
        |> Utils.asyncRead
        |> Utils.thenParse<SuccessOrErrorResponse>
