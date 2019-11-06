namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API endpoint that returns a
/// list of messages. Uses .NET types.
type IBclDeviantArtMessageCursorResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Results: seq<IBclDeviantArtMessage>

/// A single page of results from a DeviantArt API endpoint that returns a
/// list of messages.
type DeviantArtMessageCursorResult = {
    cursor: string
    has_more: bool
    results: DeviantArtMessage[]
} with
    static member Parse json = Json.deserialize<DeviantArtMessageCursorResult> json
    interface IBclDeviantArtMessageCursorResult with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Results = this.results |> Seq.map (fun o -> o :> IBclDeviantArtMessage)
    interface IResultPage<string option, DeviantArtMessage> with
        member this.HasMore = this.has_more
        member this.Cursor = Option.ofObj this.cursor
        member this.Items = this.results |> Seq.ofArray