namespace DeviantArtFs.Api.User

open DeviantArtFs
open FSharp.Control

module StatusesList =
    let AsyncExecute token common (username: string) paging =
        seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/statuses"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtStatus>>

    let ToAsyncSeq token common username offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common username)

    let ToArrayAsync token common username offset limit =
        ToAsyncSeq token common username offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common username paging =
        AsyncExecute token common username paging
        |> Async.StartAsTask