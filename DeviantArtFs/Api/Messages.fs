namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control

module Messages =
    type StackMessages = StackMessages of bool with static member Default = StackMessages true

    type MessageFolder = Inbox | MessageFolder of Guid with static member Default = Inbox

    type MessageCursor = StartingCursor | MessageCursor of string with static member Default = StartingCursor

    type MessageSubject = {
        profile: User option
        deviation: Deviation option
        status: Status option
        comment: Comment option
        collection: CollectionFolder option
        gallery: GalleryFolder option
    }

    type Message = {
        messageid: string
        ``type``: string
        orphaned: bool
        ts: DateTimeOffset option
        stackid: string option
        stack_count: int option
        is_new: bool
        originator: User option
        subject: MessageSubject option
        html: string option
        profile: User option
        deviation: Deviation option
        status: Status option
        comment: Comment option
        collection: CollectionFolder option
    }

    type MessageCursorResult = {
        cursor: string
        has_more: bool
        results: Message list
    }

    let PageFeedAsync token stack folderid cursor =
        seq {
            match stack with
            | StackMessages true -> "stack", "1"
            | StackMessages false -> "stack", "0"
            match folderid with
            | MessageFolder g -> "folderid", string g
            | Inbox -> ()
            match cursor with
            | MessageCursor s -> "cursor", s
            | StartingCursor -> ()
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/messages/feed"
        |> Utils.readAsync
        |> Utils.thenParse<MessageCursorResult>

    let GetFeedAsync token stack folderid cursor = Utils.buildAsyncSeq {
        get_page = (fun cursor -> PageFeedAsync token stack folderid cursor)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> MessageCursor page.cursor)
        initial_offset = cursor
    }

    type MessageDeletionTarget = DeleteMessage of string | DeleteStack of string

    let DeleteAsync token folderid target =
        seq {
            match folderid with
            | MessageFolder g -> "folderid", string g
            | Inbox -> ()
            match target with
            | DeleteMessage s -> "messageid", s
            | DeleteStack s -> "stackid", s
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/messages/delete"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    type FeedbackMessageType = CommentFeedbackMessages | ReplyFeedbackMessages | ActivityFeedbackMessages

    let PageFeedbackMessagesAsync token ``type`` stack folderid limit offset =
        seq {
            match ``type`` with
            | CommentFeedbackMessages -> "type", "comments"
            | ReplyFeedbackMessages -> "type", "replies"
            | ActivityFeedbackMessages -> "type", "activity"
            match stack with
            | StackMessages true -> "stack", "1"
            | StackMessages false -> "stack", "0"
            match folderid with
            | MessageFolder g -> "folderid", string g
            | Inbox -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 5
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/messages/feedback"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Message>>

    let GetFeedbackMessagesAsync token ``type`` stack folderid batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageFeedbackMessagesAsync token ``type`` stack folderid batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let PageFeedbackStackAsync token stackid limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/messages/feedback/{Uri.EscapeDataString stackid}"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Message>>

    let GetFeedbackStackAsync token stackid batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageFeedbackStackAsync token stackid batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let PageMentionsAsync token stack folderid limit offset =
        seq {
            match stack with
            | StackMessages true -> "stack", "1"
            | StackMessages false -> "stack", "0"
            match folderid with
            | MessageFolder g -> "folderid", string g
            | Inbox -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/messages/mentions"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Message>>

    let GetMentionsAsync token stack folderid batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageMentionsAsync token stack folderid batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let PageMentionsStackAsync token stackid limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/messages/mentions/{Uri.EscapeDataString stackid}"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Message>>

    let GetMentionsStackAsync token stackid batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageMentionsStackAsync token stackid batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }