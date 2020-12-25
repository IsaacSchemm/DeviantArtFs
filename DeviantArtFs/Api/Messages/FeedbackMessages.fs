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
    let AsyncExecute token common paging (req: FeedbackMessagesRequest) =
        seq {
            match req.Type with
            | FeedbackMessageType.Comments -> yield "type=comments"
            | FeedbackMessageType.Replies -> yield "type=replies"
            | FeedbackMessageType.Activity -> yield "type=activity"
            | _ -> invalidArg "req" "Invalid feedback message type"
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/messages/feedback"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (fun o -> AsyncExecute token common { Offset = o; Limit = DeviantArtPagingParams.Max } req)

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask