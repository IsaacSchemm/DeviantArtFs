namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Messages =
    let AsyncPageFeed token mode folderid cursor =
        seq {
            yield! QueryFor.messageMode mode
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.messageCursor cursor
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/feed"
        |> Dafs.asyncRead
        |> Dafs.thenParse<MessageCursorResult>

    let AsyncGetFeed token mode folderid cursor =
        Dafs.toAsyncSeq cursor (AsyncPageFeed token mode folderid)

    let AsyncDelete token folderid target =
        seq {
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.messageDeletionTarget target
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/messages/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    type FeedbackMessageType =
    | Comments = 1
    | Replies = 2
    | Activity = 3

    type FeedbackMessagesRequest(``type``: FeedbackMessageType) =
        member __.Type = ``type``
        member val Folderid = Nullable<Guid>() with get, set
        member val Stack = true with get, set

    let AsyncPageFeedbackMessages token ``type`` mode folderid limit offset =
        seq {
            yield! QueryFor.feedbackMessageType ``type``
            yield! QueryFor.messageMode mode
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 5
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/feedback"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetFeedbackMessages token ``type`` mode folderid batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageFeedbackMessages token ``type`` mode folderid batchsize)

    let AsyncPageFeedbackStack token stackid limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback/%s" (Uri.EscapeDataString stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetFeedbackStack token stackid batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageFeedbackStack token stackid batchsize)

    let AsyncPageMentions token mode folderid limit offset =
        seq {
            yield! QueryFor.messageMode mode
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/mentions"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetMentions token mode folderid batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageMentions token mode folderid batchsize)

    let AsyncPageMentionsStack token stackid limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/mentions/%s" (Uri.EscapeDataString stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetMentionsStack token stackid batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageMentionsStack token stackid batchsize)