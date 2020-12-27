namespace DeviantArtFs

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtEmbeddedContentPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool option
    prev_offset: int option
    results: Deviation list
} with
    interface IDeviantArtResultPage<DeviantArtPagingParams, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = DeviantArtPagingParams.MaxFrom (this.next_offset |> Option.defaultValue 0)
        member this.Items = this.results |> Seq.ofList