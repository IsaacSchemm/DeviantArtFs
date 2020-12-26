namespace DeviantArtFs.Api.Messages

open System
open DeviantArtFs
open FSharp.Control

type FeedbackMessageType =
| Comments = 1
| Replies = 2
| Activity = 3

type FeedbackMessagesRequest(``type``: FeedbackMessageType) =
    member __.Type = ``type``
    member val Folderid = Nullable<Guid>() with get, set
    member val Stack = true with get, set

module FeedbackMessages =
    let AsyncExecute token (req: FeedbackMessagesRequest) paging =
        seq {
            match req.Type with
            | FeedbackMessageType.Comments -> yield "type=comments"
            | FeedbackMessageType.Replies -> yield "type=replies"
            | FeedbackMessageType.Activity -> yield "type=activity"
            | _ -> invalidArg "req" "Invalid feedback message type"
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            yield! QueryFor.paging paging 5
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/feedback"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let ToAsyncSeq token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token req)

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> Async.StartAsTask