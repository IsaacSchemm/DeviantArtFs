namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtBrowsePagedResult = {
    has_more: bool
    next_offset: int option
    error_code: int option
    estimated_total: int option
    effective_page_type: string option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtBrowsePagedResult> json
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList