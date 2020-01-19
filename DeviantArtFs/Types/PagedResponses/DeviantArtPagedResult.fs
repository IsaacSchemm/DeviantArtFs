namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint with bidirectional
/// paging.
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
    member this.GetHasMore() = Nullable this.has_more
    member this.GetNextOffset() = OptUtils.toNullable this.next_offset
    member this.GetHasLess() = OptUtils.toNullable this.has_less
    member this.GetPrevOffset() = OptUtils.toNullable this.prev_offset
    interface IResultPage<int, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofArray