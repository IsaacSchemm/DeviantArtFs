namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API message feed.
type DeviantArtMessageCursorResult = {
    cursor: string
    has_more: bool
    results: DeviantArtMessage list
} with
    static member Parse json = Json.deserialize<DeviantArtMessageCursorResult> json
    interface IResultPage<string option, DeviantArtMessage> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.results |> Seq.ofList