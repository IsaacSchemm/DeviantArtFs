namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtFeedCursorResult<'a> =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Items: seq<'a>

type DeviantArtFeedCursorResult<'a> = {
    cursor: string
    has_more: bool
    items: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtFeedCursorResult<'a>> json
    interface IBclDeviantArtFeedCursorResult<'a> with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Items = this.items |> Seq.ofArray
    interface IResultPage<string option, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.items |> Seq.ofArray