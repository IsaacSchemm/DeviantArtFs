namespace DeviantArtFs

open System

type IDeviantArtPagedResult<'a> =
    inherit System.Collections.Generic.IEnumerable<'a>
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: bool
    abstract member PrevOffset: Nullable<int>
    abstract member EstimatedTotal: Nullable<int>
    abstract member Name: string
    abstract member Results: seq<'a>

type DeviantArtPagedResult<'a> = {
    HasMore: bool
    NextOffset: int option
    Results: seq<'a>
} with
    interface IDeviantArtPagedResult<'a> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = false
        member this.PrevOffset = Nullable()
        member this.EstimatedTotal = Nullable()
        member this.Name = null
        member this.Results = this.Results
        member this.GetEnumerator() = this.Results.GetEnumerator()
        member this.GetEnumerator() = this.Results.GetEnumerator() :> System.Collections.IEnumerator

type DeviantArtPagedSearchResult<'a> = {
    HasMore: bool
    NextOffset: int option
    EstimatedTotal: int option
    Results: seq<'a>
} with
    interface IDeviantArtPagedResult<'a> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = false
        member this.PrevOffset = Nullable()
        member this.EstimatedTotal = this.EstimatedTotal |> Option.toNullable
        member this.Name = null
        member this.Results = this.Results
        member this.GetEnumerator() = this.Results.GetEnumerator()
        member this.GetEnumerator() = this.Results.GetEnumerator() :> System.Collections.IEnumerator

type DeviantArtBidirectionalSearchResult<'a> = {
    HasMore: bool
    NextOffset: int option
    HasLess: bool
    PrevOffset: int option
    Results: seq<'a>
} with
    interface IDeviantArtPagedResult<'a> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = this.HasLess
        member this.PrevOffset = this.PrevOffset |> Option.toNullable
        member this.EstimatedTotal = Nullable()
        member this.Name = null
        member this.Results = this.Results
        member this.GetEnumerator() = this.Results.GetEnumerator()
        member this.GetEnumerator() = this.Results.GetEnumerator() :> System.Collections.IEnumerator

type DeviantArtGalleryResult<'a> = {
    HasMore: bool
    NextOffset: int option
    Name: string option
    Results: seq<'a>
} with
    interface IDeviantArtPagedResult<'a> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = false
        member this.PrevOffset = Nullable()
        member this.Name = this.Name |> Option.toObj
        member this.EstimatedTotal = Nullable()
        member this.Results = this.Results
        member this.GetEnumerator() = this.Results.GetEnumerator()
        member this.GetEnumerator() = this.Results.GetEnumerator() :> System.Collections.IEnumerator