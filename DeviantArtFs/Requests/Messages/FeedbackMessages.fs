namespace DeviantArtFs.Requests.Messages

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
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: FeedbackMessagesRequest) = async {
        let query = seq {
            match req.Type with
            | FeedbackMessageType.Comments -> yield "type=comments"
            | FeedbackMessageType.Replies -> yield "type=replies"
            | FeedbackMessageType.Activity -> yield "type=activity"
            | _ -> invalidArg "req" "Invalid feedback message type"
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            yield! QueryFor.paging paging 50
        }

        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback?%s"
            |> Dafs.createRequest token

        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtMessage>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtMessage)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtMessage)
        |> Async.StartAsTask