namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

type RecommendedRequest() =
     member val Q = null with get, set

module Recommended =
    let AsyncExecute token common paging (req: RecommendedRequest) =
        seq {
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtRecommendedPagedResult>

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (fun o -> AsyncExecute token common { Offset = o; Limit = DeviantArtPagingParams.Max } req)

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask