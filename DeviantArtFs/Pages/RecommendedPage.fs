namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes

type RecommendedPage = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    effective_page_type: string option
    results: Deviation list
} with
    interface ILinearPage<Deviation> with
        member this.NextOffset = this.next_offset
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results