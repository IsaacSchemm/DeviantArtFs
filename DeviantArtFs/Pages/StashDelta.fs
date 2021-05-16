namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes

type StashDelta = {
    cursor: string
    has_more: bool
    next_offset: int option
    reset: bool
    entries: StashDeltaEntry list
} with
    interface ILinearPage<StashDeltaEntry> with
        member this.NextOffset = this.next_offset
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.entries