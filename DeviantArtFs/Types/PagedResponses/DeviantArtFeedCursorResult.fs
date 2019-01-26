namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtFeedCursorResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Items: seq<IBclDeviantArtFeedItem>

type DeviantArtFeedCursorResult = {
    cursor: string
    has_more: bool
    items: DeviantArtFeedItem[]
} with
    static member Parse json = Json.deserialize<DeviantArtFeedCursorResult> json
    interface IBclDeviantArtFeedCursorResult with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Items = this.items |> Seq.map (fun o -> o :> IBclDeviantArtFeedItem)
    interface IResultPage<string option, DeviantArtFeedItem> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.items |> Seq.ofArray