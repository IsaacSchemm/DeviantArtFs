namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module PostsByDeviantsYouWatch =
    let AsyncExecute token common paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtPost>>

    let ToAsyncSeq token common offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common)

    let ToArrayAsync token common offset limit =
        ToAsyncSeq token common offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging =
        AsyncExecute token common paging
        |> Async.StartAsTask