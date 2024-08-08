namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control

module Gallery =
    let PageAllViewAsync token scope limit offset =
        seq {
            yield! QueryFor.userScope scope
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/gallery/all"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Deviation>>

    let GetAllViewAsync token scope batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageAllViewAsync token scope batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let PageGalleryAsync token scope folder limit offset =
        let folder_id_str =
            match folder with
            | SingleGalleryFolder s -> Utils.guidString s
            | AllGalleryFoldersNewest | AllGalleryFoldersPopular -> ""

        seq {
            yield! QueryFor.userScope scope
            match folder with
            | SingleGalleryFolder _ -> ()
            | AllGalleryFoldersNewest -> "mode", "newest"
            | AllGalleryFoldersPopular -> "mode", "popular"
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/gallery/{folder_id_str}"
        |> Utils.readAsync
        |> Utils.thenParse<FolderPage>

    let GetGalleryAsync token scope folder batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageGalleryAsync token scope folder batchsize offset)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let PageFoldersAsync token calculateSize extPreload filterEmptyFolder user limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.calculateSize calculateSize
            yield! QueryFor.folderPreload extPreload
            yield! QueryFor.filterEmptyFolder filterEmptyFolder
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/gallery/folders"
        |> Utils.readAsync
        |> Utils.thenParse<Page<GalleryFolder>>

    let GetFoldersAsync token calculateSize extPreload filterEmptyFolder user batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageFoldersAsync token calculateSize extPreload filterEmptyFolder user batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let CopyDeviationsAsync token target_folderid deviationids =
        seq {
            "target_folderid", Utils.guidString target_folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/copy_deviations"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    type FolderDescription = FolderDescription of string | NoDescription
    type ParentFolderId = ParentFolderId of Guid | NoParentGallery

    let CreateFolderAsync token folder description parent_folderid =
        seq {
            yield "folder", folder
            match description with
            | FolderDescription d -> yield "description", d
            | NoDescription -> ()
            match parent_folderid with
            | ParentFolderId g -> yield "parent_folderid", Utils.guidString g
            | NoParentGallery -> ()
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        |> Utils.readAsync
        |> Utils.thenParse<CollectionFolder>

    let MoveFolderAsync token folderid to_folderid =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "to_folderid", Utils.guidString to_folderid
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/move"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let MoveDeviationsAsync token source_folderid target_folderid deviationids =
        seq {
            "source_folderid", Utils.guidString source_folderid
            "target_folderid", Utils.guidString target_folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/move_deviations"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let RemoveFolderAsync token folderid =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/{Utils.guidString folderid}"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let RemoveDeviationsAsync token folderid deviationids =
        seq {
            "folderid", Utils.guidString folderid
            for d in deviationids do
                "deviationids[]", Utils.guidString d
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove_deviations"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let UpdateFolderAsync token folderid folderUpdates =
        seq {
            yield "folderid", Utils.guidString folderid
            yield! QueryFor.folderUpdates folderUpdates
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/update"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let UpdateDeviationOrderAsync token folderid deviationid position =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "deviationid", deviationid
            yield "position", Utils.intString position
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/update_deviation_order"
        |> Utils.readAsync
        |> Utils.thenMap ignore

    let UpdateOrderAsync token folderid position =
        seq {
            yield "folderid", Utils.guidString folderid
            yield "position", Utils.intString position
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/update_order"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>