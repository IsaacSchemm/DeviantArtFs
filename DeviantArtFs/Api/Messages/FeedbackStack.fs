namespace DeviantArtFs.Api.Messages

open DeviantArtFs
open FSharp.Control
open System

module FeedbackStack =
    let AsyncExecute token common paging (stackid: string) =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback/%s" (Dafs.urlEncode stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let private AsyncGetPage token common stackid cursor =
        AsyncExecute token common { Offset = cursor; Limit = Nullable Int32.MaxValue } stackid

    let ToAsyncSeq token common offset stackid =
        Dafs.toAsyncSeq3 offset (AsyncGetPage token common stackid)

    let ToArrayAsync token common offset limit stackid =
        ToAsyncSeq token common offset stackid
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging stackid =
        AsyncExecute token common paging stackid
        |> Async.StartAsTask