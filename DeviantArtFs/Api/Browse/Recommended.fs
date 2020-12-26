namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

type RecommendedRequest() =
     member val Q = null with get, set

module Recommended =
    let AsyncExecute token expansion (req: RecommendedRequest) paging =
        seq {
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtRecommendedPagedResult>

    let ToAsyncSeq token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion req)

    let ToArrayAsync token expansion req offset limit =
        ToAsyncSeq token expansion req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion req paging =
        AsyncExecute token expansion req paging
        |> Async.StartAsTask