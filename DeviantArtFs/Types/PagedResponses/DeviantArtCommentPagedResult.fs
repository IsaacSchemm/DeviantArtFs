namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint that fetches
/// comments.
type DeviantArtCommentPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    total: int option
    thread: DeviantArtComment list
} with
    static member Parse json = Json.deserialize<DeviantArtCommentPagedResult> json
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    member this.GetPrevOffset() = OptUtils.intDefault this.prev_offset
    member this.GetTotal() = OptUtils.intDefault this.total
    interface IResultPage<int, DeviantArtComment> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.thread |> Seq.ofList