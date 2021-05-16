namespace DeviantArtFs

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtFolderPagedResult = {
    has_more: bool
    next_offset: int option
    name: string option
    results: Deviation list
} with
    interface IDeviantArtResultPage<ParameterTypes.PagingOffset, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.map ParameterTypes.PagingOffset |> Option.defaultValue ParameterTypes.FromStart
        member this.Items = this.results |> Seq.ofList