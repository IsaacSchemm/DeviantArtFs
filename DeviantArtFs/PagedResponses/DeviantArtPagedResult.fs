namespace DeviantArtFs

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    error_code: int option
    results: 'a list option
} with
    interface IDeviantArtResultPage<ParameterTypes.PagingOffset, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.map ParameterTypes.PagingOffset |> Option.defaultValue ParameterTypes.FromStart
        member this.Items = this.results |> Option.defaultValue List.empty |> Seq.ofList