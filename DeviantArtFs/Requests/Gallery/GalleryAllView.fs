namespace DeviantArtFs.Requests.Gallery

open DeviantArtFs

type GalleryAllViewRequest() =
    member val Username = null with get, set

module GalleryAllView =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: GalleryAllViewRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/all?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 24 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> iop.thenMapResult (fun o -> o :> IBclDeviation) |> Async.StartAsTask
