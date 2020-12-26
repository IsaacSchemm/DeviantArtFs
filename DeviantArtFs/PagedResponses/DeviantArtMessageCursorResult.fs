namespace DeviantArtFs

/// A single page of results from a DeviantArt API message feed.
type DeviantArtMessageCursorResult = {
    cursor: string
    has_more: bool
    results: DeviantArtMessage list
} with
    interface IResultPage<string option, DeviantArtMessage> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.results |> Seq.ofList