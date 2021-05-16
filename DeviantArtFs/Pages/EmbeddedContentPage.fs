namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes

type EmbeddedContentPage = {
    has_more: bool
    next_offset: int option
    has_less: bool option
    prev_offset: int option
    results: Deviation list
} with
    interface ILinearPage<Deviation> with
        member this.NextOffset = this.next_offset
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results