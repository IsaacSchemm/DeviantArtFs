namespace DeviantArtFs.Api.Stash

open DeviantArtFs
open FSharp.Control

module Contents =
    let RootStack = 0L

    let AsyncExecute token common (stackid: int64) paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<StashMetadata>>

    let ToAsyncSeq token common stackid offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common stackid)

    let ToArrayAsync token common stackid offset limit =
        ToAsyncSeq token common stackid offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common stackid paging =
        AsyncExecute token common stackid paging
        |> Async.StartAsTask