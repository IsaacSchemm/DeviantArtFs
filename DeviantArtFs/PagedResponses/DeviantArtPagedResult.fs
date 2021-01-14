namespace DeviantArtFs

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    error_code: int option
    results: 'a list option
} with
    interface IDeviantArtResultPage<DeviantArtPagingParams, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = DeviantArtPagingParams.MaxFrom (this.next_offset |> Option.defaultValue 0)
        member this.Items = this.results |> Option.defaultValue List.empty |> Seq.ofList