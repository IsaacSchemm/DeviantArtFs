namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API endpoint that uses a string
/// cursor for pagination.
type DeviantArtFeedCursorResult = {
    cursor: string
    has_more: bool
    items: DeviantArtFeedItem list
} with
    static member Parse json = Json.deserialize<DeviantArtFeedCursorResult> json
    interface IResultPage<string option, DeviantArtFeedItem> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.items |> Seq.ofList