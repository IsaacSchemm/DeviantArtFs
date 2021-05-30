namespace DeviantArtFs.Pages

open DeviantArtFs.ResponseTypes
open DeviantArtFs.ParameterTypes

type MessageCursorResult = {
    cursor: string
    has_more: bool
    results: Message list
} with
    interface IPage<MessageCursor, Message> with
        member this.NextPage =
            match this.has_more with
            | true -> Some (MessageCursor this.cursor)
            | false -> None
        member this.Items = this.results