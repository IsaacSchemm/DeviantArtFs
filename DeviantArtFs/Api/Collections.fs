namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes

module Collections =
    let AsyncPageCollection token expansion user folderid paging =
        seq {
            match user with
            | CollectionsForUser s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | CollectionsForCurrentUser -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%s" (Dafs.guid2str folderid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFolderPagedResult>

    let AsyncGetCollection token expansion user folderid offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageCollection token expansion user folderid)

    let AsyncPageFolders token calculateSize extPreload username paging =
        seq {
            match username with
            | CollectionsForUser s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | CollectionsForCurrentUser -> ()
            match calculateSize with
            | CollectionsFolderCalculateSize true -> yield "calculate_size=1"
            | CollectionsFolderCalculateSize false -> yield "calculate_size=0"
            match extPreload with
            | CollectionsFolderPreload true -> yield "ext_preload=1"
            | CollectionsFolderPreload false -> yield "ext_preload=0"
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtCollectionFolder>>

    let AsyncGetFolders token extPreload calculateSize username offset =
        Dafs.toAsyncSeq offset (AsyncPageFolders token extPreload calculateSize username)

    let AsyncFave token deviationid folderids =
        seq {
            yield sprintf "deviationid=%s" (Dafs.guid2str deviationid)
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index (Dafs.guid2str f)
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFaveResult>

    let AsyncUnfave token deviationid folderids =
        seq {
            yield sprintf "deviationid=%s" (Dafs.guid2str deviationid)
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index (Dafs.guid2str f)
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/unfave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFaveResult>

    let AsyncCreateFolder token folder =
        seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCollectionFolder>

    let AsyncRemoveFolder token folderid =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%s" (Dafs.guid2str folderid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>