namespace DeviantArtFs.Requests.Gallery

open DeviantArtFs

type GalleryFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val ExtPreload = false with get, set

module GalleryFolders =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: GalleryFoldersRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield sprintf "ext_preload=%b" req.ExtPreload
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtGalleryFolder>.Parse json
    }

    let AsyncGetMax token offset req =
        let paging = Dafs.page offset 50
        AsyncExecute token paging req

    let ToAsyncSeq token offset req =
        AsyncGetMax token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun f -> f :> IBclDeviantArtGalleryFolder)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtGalleryFolder)
        |> Async.StartAsTask

    let GetMaxAsync token paging req =
        AsyncGetMax token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtGalleryFolder)
        |> Async.StartAsTask