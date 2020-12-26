namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module PostsByDeviantsYouWatch =
    let AsyncExecute token paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtPost>>

    let ToAsyncSeq token offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token)

    let ToArrayAsync token offset limit =
        ToAsyncSeq token offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging =
        AsyncExecute token paging
        |> Async.StartAsTask