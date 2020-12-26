namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

type NewestRequest() =
    member val CategoryPath = null with get, set
    member val Q = null with get, set

module Newest =
    let AsyncExecute token common (req: NewestRequest) paging =
        seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (Dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/newest"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

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