namespace DeviantArtFs.Api.User

open DeviantArtFs
open FSharp.Control

module StatusesList =
    let AsyncExecute token (username: string) paging =
        seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/statuses"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtStatus>>

    let ToAsyncSeq token username offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token username)

    let ToArrayAsync token username offset limit =
        ToAsyncSeq token username offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token username paging =
        AsyncExecute token username paging
        |> Async.StartAsTask