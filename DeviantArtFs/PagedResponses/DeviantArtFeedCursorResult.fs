namespace DeviantArtFs

/// A single page of results from a DeviantArt API feed.
type DeviantArtFeedCursorResult = {
    cursor: string
    has_more: bool
    items: DeviantArtFeedItem list
} with
    interface IDeviantArtResultPage<string option, DeviantArtFeedItem> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.items |> Seq.ofList