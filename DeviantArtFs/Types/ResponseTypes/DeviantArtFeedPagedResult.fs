namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtFeedPagedResult<'a> =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Items: seq<'a>

type DeviantArtFeedPagedResult<'a> = {
    cursor: string
    has_more: bool
    items: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtFeedPagedResult<'a>> json
    interface IBclDeviantArtFeedPagedResult<'a> with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Items = this.items |> Seq.ofArray