namespace DeviantArtFs.Api.Gallery

open DeviantArtFs

type GalleryAllViewRequest() =
    member val Username = null with get, set

module GalleryAllView =
    open FSharp.Control

    let AsyncExecute token paging (req: GalleryAllViewRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/all?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask