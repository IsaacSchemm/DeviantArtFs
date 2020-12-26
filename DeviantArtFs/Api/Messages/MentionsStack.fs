namespace DeviantArtFs.Api.Messages

open DeviantArtFs
open FSharp.Control

module MentionsStack =
    let AsyncExecute token common (stackid: string) paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/mentions/%s" (Dafs.urlEncode stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

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