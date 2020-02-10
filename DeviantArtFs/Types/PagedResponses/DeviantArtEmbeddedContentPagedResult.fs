namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtEmbeddedContentPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool option
    prev_offset: int option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtEmbeddedContentPagedResult> json
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    member this.GetHasLess() = OptUtils.boolDefault this.has_less
    member this.GetPrevOffset() = OptUtils.intDefault this.prev_offset
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList