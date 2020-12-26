namespace DeviantArtFs.Api.Gallery

open DeviantArtFs
open FSharp.Control

type GalleryAllViewRequest() =
    member val Username = null with get, set

module GalleryAllView =
    let AsyncExecute token common (req: GalleryAllViewRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/gallery/all"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq3 (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common req)

    let ToArrayAsync token common req offset limit =
        ToAsyncSeq token common req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask