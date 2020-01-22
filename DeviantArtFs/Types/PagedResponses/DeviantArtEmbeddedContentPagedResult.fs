namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type IBclDeviantArtEmbeddedContentPagedResult =
    inherit IBclDeviantArtPagedResult<IBclDeviation>

    /// Whether there are more results on the previous page, if the request supports bidirectional paging.
    abstract member HasLess: Nullable<bool>
    /// The previous page's offset, if any.
    abstract member PrevOffset: Nullable<int>

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtEmbeddedContentPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool option
    prev_offset: int option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtEmbeddedContentPagedResult> json
    interface IBclDeviantArtEmbeddedContentPagedResult with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.HasLess = this.has_less |> Option.toNullable
        member this.PrevOffset = this.prev_offset |> Option.toNullable
        member this.Results = this.results |> Seq.map (fun o -> o :> IBclDeviation)
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList