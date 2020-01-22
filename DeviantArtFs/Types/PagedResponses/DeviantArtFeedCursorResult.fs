namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API feed.
type IBclDeviantArtFeedCursorResult =
    /// The cursor to provide as part of the next request to get the next page.
    abstract member Cursor: string
    /// Whether there are more results on the next page.
    abstract member HasMore: bool
    /// A list of feed items.
    abstract member Items: seq<IBclDeviantArtFeedItem>

/// A single page of results from a DeviantArt API feed.
type DeviantArtFeedCursorResult = {
    cursor: string
    has_more: bool
    items: DeviantArtFeedItem list
} with
    static member Parse json = Json.deserialize<DeviantArtFeedCursorResult> json
    interface IBclDeviantArtFeedCursorResult with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Items = this.items |> Seq.map (fun o -> o :> IBclDeviantArtFeedItem)
    interface IResultPage<string option, DeviantArtFeedItem> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.items |> Seq.ofList