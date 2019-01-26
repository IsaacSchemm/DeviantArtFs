namespace DeviantArtFs

open System
open FSharp.Json

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
    static member Map (f: 'a -> 'b) (r: IBclDeviantArtPagedResult<'a>) = {
        new IBclDeviantArtPagedResult<'b> with
            member __.HasMore = r.HasMore
            member __.NextOffset = r.NextOffset
            member __.HasLess = r.HasLess
            member __.PrevOffset = r.PrevOffset
            member __.EstimatedTotal = r.EstimatedTotal
            member __.Name = r.Name
            member __.Results = Seq.map f r.Results
    }
    interface IBclDeviantArtPagedResult<'a> with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.HasLess = this.has_less |> Option.toNullable
        member this.PrevOffset = this.prev_offset |> Option.toNullable
        member this.EstimatedTotal = this.estimated_total |> Option.toNullable
        member this.Name = this.name |> Option.toObj
        member this.Results = this.results |> Seq.ofArray
    interface IResultPage<int, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofArray