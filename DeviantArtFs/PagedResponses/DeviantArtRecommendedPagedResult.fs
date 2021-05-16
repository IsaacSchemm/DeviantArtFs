namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtRecommendedPagedResult = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    effective_page_type: string option
    results: Deviation list
} with
    interface IDeviantArtResultPage<PagingOffset, Deviation> with
        member this.Cursor =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results |> Seq.ofList