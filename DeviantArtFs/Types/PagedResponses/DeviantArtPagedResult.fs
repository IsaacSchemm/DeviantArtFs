namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type IBclDeviantArtPagedResult<'a> =
    /// Whether there are more results on the next page.
    abstract member HasMore: bool
    /// The next page's offset, if any.
    abstract member NextOffset: Nullable<int>
    /// Whether there are more results on the previous page, if the request supports bidirectional paging.
    abstract member HasLess: Nullable<bool>
    /// The previous page's offset, if any.
    abstract member PrevOffset: Nullable<int>
    /// The estimated total number of results, if provided.
    abstract member EstimatedTotal: Nullable<int>
    /// The name, if provided.
    abstract member Name: string
    /// A list of results.
    abstract member Results: seq<'a>

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    has_less: bool option
    prev_offset: int option
    name: string option
    results: 'a list
} with
    static member Parse json = Json.deserialize<DeviantArtPagedResult<'a>> json
    interface IBclDeviantArtPagedResult<'a> with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.HasLess = this.has_less |> Option.toNullable
        member this.PrevOffset = this.prev_offset |> Option.toNullable
        member this.EstimatedTotal = this.estimated_total |> Option.toNullable
        member this.Name = this.name |> Option.toObj
        member this.Results = this.results |> Seq.ofList
    interface IResultPage<int, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList