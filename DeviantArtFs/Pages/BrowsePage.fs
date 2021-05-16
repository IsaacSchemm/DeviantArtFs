namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes

type BrowsePage = {
    has_more: bool
    next_offset: int option
    error_code: int option
    estimated_total: int option
    results: Deviation list
} with
    interface ILinearPage<Deviation> with
        member this.NextOffset = this.next_offset
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results