namespace DeviantArtFs

open System

type IDeviantArtPagedResult<'a> =
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: Nullable<bool>
    abstract member PrevOffset: Nullable<int>
    abstract member EstimatedTotal: Nullable<int>
    abstract member Name: string
    abstract member Results: seq<'a>

type DeviantArtPagedResult<'a> = {
    HasMore: bool
    NextOffset: int option
    EstimatedTotal: int option
    HasLess: bool option
    PrevOffset: int option
    Name: string option
    Results: seq<'a>
} with
    interface IDeviantArtPagedResult<'a> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = this.HasLess |> Option.toNullable
        member this.PrevOffset = this.PrevOffset |> Option.toNullable
        member this.EstimatedTotal = this.EstimatedTotal |> Option.toNullable
        member this.Name = this.Name |> Option.toObj
        member this.Results = this.Results