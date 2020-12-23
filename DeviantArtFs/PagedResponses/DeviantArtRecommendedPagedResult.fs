namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtRecommendedPagedResult = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    effective_page_type: string option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtRecommendedPagedResult> json
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList