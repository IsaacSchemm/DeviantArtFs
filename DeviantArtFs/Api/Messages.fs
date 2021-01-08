namespace DeviantArtFs.Api

open System
open DeviantArtFs

module Messages =
    type MessagesFeedRequest() =
        member val Folderid = Nullable<Guid>() with get, set
        member val Stack = true with get, set

    let AsyncPageFeed token (req: MessagesFeedRequest) (cursor: string option) =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            match cursor with
            | Some c -> yield sprintf "cursor=%s" c
            | None -> ()
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/feed"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMessageCursorResult>

    let AsyncGetFeed token req cursor =
        Dafs.toAsyncSeq cursor (AsyncPageFeed token req)

    type DeleteMessageRequest() =
        member val Folderid = Nullable<Guid>() with get, set
        member val Messageid = null with get, set
        member val Stackid = null with get, set

    let AsyncDelete token (req: DeleteMessageRequest) =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            if req.Messageid <> null then
                yield sprintf "messageid=%s" req.Messageid
            if req.Stackid <> null then
                yield sprintf "stackid=%s" req.Stackid
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/messages/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    type FeedbackMessageType =
    | Comments = 1
    | Replies = 2
    | Activity = 3

    type FeedbackMessagesRequest(``type``: FeedbackMessageType) =
        member __.Type = ``type``
        member val Folderid = Nullable<Guid>() with get, set
        member val Stack = true with get, set

    let AsyncPageFeedbackMessages token (req: FeedbackMessagesRequest) paging =
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

    let AsyncGetFeedbackMessages token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageFeedbackMessages token req)

    let AsyncPageFeedbackStack token (stackid: string) paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback/%s" (Dafs.urlEncode stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let AsyncGetFeedbackStack token offset stackid =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageFeedbackStack token stackid)
        
    type MentionsMessagesRequest() =
        member val Folderid = Nullable<Guid>() with get, set
        member val Stack = true with get, set

    let AsyncPageMentions token (req: MentionsMessagesRequest) paging =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/mentions"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let AsyncGetMentions token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageMentions token req)

    let AsyncPageMentionsStack token (stackid: string) paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/mentions/%s" (Dafs.urlEncode stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let AsyncGetMentionsStack token stackid offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageMentionsStack token stackid)