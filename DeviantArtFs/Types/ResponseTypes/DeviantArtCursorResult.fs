namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtCursorResult<'a> =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Items: seq<'a>

type DeviantArtCursorResult<'a> = {
    cursor: string
    has_more: bool
    items: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtCursorResult<'a>> json
    interface IBclDeviantArtCursorResult<'a> with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Items = this.items |> Seq.ofArray