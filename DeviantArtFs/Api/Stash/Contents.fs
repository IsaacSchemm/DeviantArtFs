namespace DeviantArtFs.Api.Stash

open DeviantArtFs
open FSharp.Control

module Contents =
    let RootStack = 0L

    let AsyncExecute token (stackid: int64) paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<StashMetadata>>

    let ToAsyncSeq token stackid offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token stackid)

    let ToArrayAsync token stackid offset limit =
        ToAsyncSeq token stackid offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token stackid paging =
        AsyncExecute token stackid paging
        |> Async.StartAsTask