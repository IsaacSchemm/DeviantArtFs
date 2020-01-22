namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint with bidirectional
/// paging. Uses .NET types.
type IBclDeviantArtPagedResult<'a> =
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: Nullable<bool>
    abstract member PrevOffset: Nullable<int>
    abstract member EstimatedTotal: Nullable<int>
    abstract member Name: string
    abstract member Results: seq<'a>

/// A single page of results from a DeviantArt API endpoint with bidirectional
/// paging.
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
    member this.Map (f: 'a -> 'b) = {
        has_more = this.has_more
        next_offset = this.next_offset
        estimated_total = this.estimated_total
        has_less = this.has_less
        prev_offset = this.prev_offset
        name = this.name
        results = List.map f this.results
    }
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