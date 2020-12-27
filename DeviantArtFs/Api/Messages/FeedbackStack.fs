namespace DeviantArtFs.Api.Messages

open DeviantArtFs
open FSharp.Control

module FeedbackStack =
    let AsyncExecute token (stackid: string) paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback/%s" (Dafs.urlEncode stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let ToAsyncSeq token offset stackid =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token stackid)

    let ToArrayAsync token stackid offset limit =
        ToAsyncSeq token stackid offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token stackid paging =
        AsyncExecute token stackid paging
        |> Async.StartAsTask