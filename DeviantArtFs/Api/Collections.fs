namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control

module Collections =
    let PageCollectionAsync token user folderid limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/collections/{Utils.guidString folderid}"
        |> Utils.readAsync
        |> Utils.thenParse<FolderPage>

#if NET
    let GetCollectionAsync token user folderid batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageCollectionAsync token user folderid batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }
#endif

    let PageAllAsync token user limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/collections/all"
        |> Utils.readAsync
        |> Utils.thenParse<FolderPage>

#if NET
    let GetAllAsync token user batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageAllAsync token user batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }
#endif

    let PageFoldersAsync token calculateSize extPreload filterEmptyFolder user limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.calculateSize calculateSize
            yield! QueryFor.folderPreload extPreload
            yield! QueryFor.filterEmptyFolder filterEmptyFolder
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Utils.readAsync
        |> Utils.thenParse<Page<CollectionFolder>>

#if NET
    let GetFoldersAsync token calculateSize extPreload filterEmptyFolder user batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageFoldersAsync token calculateSize extPreload filterEmptyFolder user batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }
#endif

    let CopyDeviationsAsync token target_folderid deviationids =
        seq {
            "target_folderid", Utils.guidString target_folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/copy_deviations"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let MoveDeviationsAsync token source_folderid target_folderid deviationids =
        seq {
            "source_folderid", Utils.guidString source_folderid
            "target_folderid", Utils.guidString target_folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/move_deviations"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let RemoveDeviationsAsync token folderid deviationids =
        seq {
            "folderid", Utils.guidString folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/move_deviations"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    type FaveResult = {
        success: bool
        favourites: int
    }

    let FaveAsync token deviationid folderids =
        seq {
            yield "deviationid", Utils.guidString deviationid
            let mutable index = 0
            for f in folderids do
                yield $"folderid[{index}]", Utils.guidString f
                index <- index + 1
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        |> Utils.readAsync
        |> Utils.thenParse<FaveResult>

    let UnfaveAsync token deviationid folderids =
        seq {
            yield "deviationid", Utils.guidString deviationid
            let mutable index = 0
            for f in folderids do
                yield $"folderid[{index}]", Utils.guidString f
                index <- index + 1
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/unfave"
        |> Utils.readAsync
        |> Utils.thenParse<FaveResult>

    let CreateFolderAsync token folder =
        seq {
            yield "folder", folder
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create"
        |> Utils.readAsync
        |> Utils.thenParse<CollectionFolder>

    let RemoveFolderAsync token folderid =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/{Utils.guidString folderid}"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let UpdateFolderAsync token folderid folderUpdates =
        seq {
            yield "folderid", Utils.guidString folderid
            yield! QueryFor.folderUpdates folderUpdates
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/update"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let UpdateDeviationOrderAsync token folderid deviationid position =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "deviationid", deviationid
            yield "position", Utils.intString position
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/update_deviation_order"
        |> Utils.readAsync
        |> Utils.thenMap ignore

    let UpdateOrderAsync token folderid position =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "position", Utils.intString position
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/collections/folders/update_order"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>