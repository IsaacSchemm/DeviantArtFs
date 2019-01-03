namespace DeviantArtFs

open System

type DeviantArtPagedResult<'a> = {
    HasMore: bool
    NextOffset: int option
    Results: seq<'a>
} with
    member this.GetNextOffset() = this.NextOffset |> Option.toNullable

type DeviantArtPagedSearchResult<'a> = {
    HasMore: bool
    NextOffset: int option
    EstimatedTotal: int option
    Results: seq<'a>
} with
    member this.GetNextOffset() = this.NextOffset |> Option.toNullable
    member this.GetEstimatedTotal() = this.EstimatedTotal |> Option.toNullable