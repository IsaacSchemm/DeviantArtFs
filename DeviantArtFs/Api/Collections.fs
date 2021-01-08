namespace DeivantArtFs.Api

open System
open DeviantArtFs

module Collections =
    type CollectionRequest(folderid: Guid) =
        member __.Folderid = folderid
        member val Username = null with get, set

    let AsyncPageCollection token expansion (req: CollectionRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%A" req.Folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFolderPagedResult>

    let AsyncGetCollection token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageCollection token expansion req)

    type CollectionFoldersRequest() =
        member val Username = null with get, set
        member val CalculateSize = false with get, set
        member val ExtPreload = false with get, set

    let AsyncPageFolders token (req: CollectionFoldersRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield sprintf "ext_preload=%b" req.ExtPreload
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtCollectionFolder>>

    let AsyncGetFolders token req offset =
        Dafs.toAsyncSeq offset (AsyncPageFolders token req)

    let AsyncFave token (deviationid: Guid) (folderids: seq<Guid>) =
        seq {
            yield sprintf "deviationid=%O" deviationid
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFaveResult>

    let AsyncUnfave token (deviationid: Guid) (folderids: seq<Guid>) =
        seq {
            yield sprintf "deviationid=%O" deviationid
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/unfave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFaveResult>

    let AsyncCreateFolder token (folder: string) =
        seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCollectionFolder>

    let AsyncRemoveFolder token (folderid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>