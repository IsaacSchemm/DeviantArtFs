namespace DeviantArtFs

open System
open FSharp.Json

type internal IDeviantArtConvertiblePagedResult<'a> =
    abstract member has_more: bool
    abstract member next_offset: int option
    abstract member enumerable: seq<'a>

type IBclDeviantArtPagedResult<'a> =
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: Nullable<bool>
    abstract member PrevOffset: Nullable<int>
    abstract member EstimatedTotal: Nullable<int>
    abstract member Name: string
    abstract member Results: seq<'a>

type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    has_less: bool option
    prev_offset: int option
    name: string option
    results: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtPagedResult<'a>> json
    interface IBclDeviantArtPagedResult<'a> with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.HasLess = this.has_less |> Option.toNullable
        member this.PrevOffset = this.prev_offset |> Option.toNullable
        member this.EstimatedTotal = this.estimated_total |> Option.toNullable
        member this.Name = this.name |> Option.toObj
        member this.Results = this.results |> Seq.ofArray
    interface IDeviantArtConvertiblePagedResult<'a> with
        member this.has_more = this.has_more
        member this.next_offset = this.next_offset
        member this.enumerable = this.results |> Seq.ofArray