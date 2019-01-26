namespace DeviantArtFs

open FSharp.Json

type internal IDeviantArtConvertibleCursorResult<'cursor, 'item> =
    abstract member cursor: 'cursor
    abstract member has_more: bool
    abstract member enumerable: seq<'item>

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
    interface IDeviantArtConvertibleCursorResult<string, 'a> with
        member this.cursor = this.cursor
        member this.has_more = this.has_more
        member this.enumerable = this.items |> Seq.ofArray