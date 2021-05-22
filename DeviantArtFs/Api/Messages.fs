namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Messages =
    let AsyncPageFeed token stack folderid cursor =
        seq {
            yield! QueryFor.stackMessages stack
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.messageCursor cursor
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/feed"
        |> Dafs.asyncRead
        |> Dafs.thenParse<MessageCursorResult>

    let AsyncGetFeed token stack folderid cursor =
        Dafs.toAsyncEnum cursor (AsyncPageFeed token stack folderid)

    let AsyncDelete token folderid target =
        seq {
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.messageDeletionTarget target
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/messages/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncPageFeedbackMessages token ``type`` stack folderid limit offset =
        seq {
            yield! QueryFor.feedbackMessageType ``type``
            yield! QueryFor.stackMessages stack
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 5
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/feedback"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetFeedbackMessages token ``type`` stack folderid batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageFeedbackMessages token ``type`` stack folderid batchsize)

    let AsyncPageFeedbackStack token stackid limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback/%s" (Uri.EscapeDataString stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetFeedbackStack token stackid batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageFeedbackStack token stackid batchsize)

    let AsyncPageMentions token stack folderid limit offset =
        seq {
            yield! QueryFor.stackMessages stack
            yield! QueryFor.messageFolder folderid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/mentions"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetMentions token stack folderid batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageMentions token stack folderid batchsize)

    let AsyncPageMentionsStack token stackid limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/mentions/%s" (Uri.EscapeDataString stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Message>>

    let AsyncGetMentionsStack token stackid batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageMentionsStack token stackid batchsize)