namespace DeviantArtFs.Pages

open DeviantArtFs.ResponseTypes

type MessageCursorResult = {
    cursor: string
    has_more: bool
    results: Message list
} with
    interface IPage<string option, Message> with
        member this.NextPage =
            match this.has_more with
            | true -> Some (Some this.cursor)
            | false -> None
        member this.Items = this.results