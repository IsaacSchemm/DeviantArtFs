namespace DeviantArtFs

open FSharp.Json

type internal IDeviantArtConvertibleCursorResult<'a> =
    abstract member cursor: string
    abstract member has_more: bool
    abstract member enumerable: seq<'a>

type IBclDeviantArtFeedCursorResult<'a> =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Results: seq<'a>

type DeviantArtFeedCursorResult<'a> = {
    cursor: string
    has_more: bool
    results: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtFeedCursorResult<'a>> json
    interface IBclDeviantArtFeedCursorResult<'a> with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Results = this.results |> Seq.ofArray
    interface IDeviantArtConvertibleCursorResult<'a> with
        member this.cursor = this.cursor
        member this.has_more = this.has_more
        member this.enumerable = this.results |> Seq.ofArray