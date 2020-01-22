namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type IBclDeviantArtPagedResult<'a> =
    /// Whether there are more results on the next page.
    abstract member HasMore: bool
    /// The next page's offset, if any.
    abstract member NextOffset: Nullable<int>
    /// A list of results.
    abstract member Results: seq<'a>

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    results: 'a list
} with
    static member Parse json = Json.deserialize<DeviantArtPagedResult<'a>> json
    interface IBclDeviantArtPagedResult<'a> with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.Results = this.results |> Seq.ofList
    interface IResultPage<int, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList