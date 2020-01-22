namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type IBclDeviantArtBrowsePagedResult =
    inherit IBclDeviantArtPagedResult<IBclDeviation>

    /// The estimated total number of results, if provided.
    abstract member EstimatedTotal: Nullable<int>

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtBrowsePagedResult = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtBrowsePagedResult> json
    interface IBclDeviantArtBrowsePagedResult with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.EstimatedTotal = this.estimated_total |> Option.toNullable
        member this.Results = this.results |> Seq.map (fun o -> o :> IBclDeviation)
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList