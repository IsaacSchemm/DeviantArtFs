namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module DeviantsYouWatch =
    let AsyncExecute token paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

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