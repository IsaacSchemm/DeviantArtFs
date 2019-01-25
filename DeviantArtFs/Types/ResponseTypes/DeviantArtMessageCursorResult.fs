namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtMessageCursorResult<'a> =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Results: seq<'a>

type DeviantArtMessageCursorResult<'a> = {
    cursor: string
    has_more: bool
    results: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtMessageCursorResult<'a>> json
    interface IBclDeviantArtMessageCursorResult<'a> with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Results = this.results |> Seq.ofArray