namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Tags =
    let AsyncExecute token common (tag: string) paging =
        seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let ToAsyncSeq token common tag offset =
        Dafs.toAsyncSeq3 (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common tag)

    let ToArrayAsync token common tag offset limit =
        ToAsyncSeq token common tag offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common tag paging =
        AsyncExecute token common tag paging
        |> Async.StartAsTask