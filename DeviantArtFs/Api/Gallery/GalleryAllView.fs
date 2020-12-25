namespace DeviantArtFs.Api.Gallery

open System
open DeviantArtFs
open FSharp.Control

type GalleryAllViewRequest() =
    member val Username = null with get, set

module GalleryAllView =
    let AsyncExecute token common paging (req: GalleryAllViewRequest) =
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

    let private AsyncGetPage token common req cursor =
        AsyncExecute token common { Offset = cursor; Limit = Nullable Int32.MaxValue } req

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (AsyncGetPage token common req)

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask