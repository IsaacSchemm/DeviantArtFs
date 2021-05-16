namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

type DeviantArtCommentSiblingsContext = {
    parent: DeviantArtComment option
    item_profile: DeviantArtUser option
    item_deviation: Deviation option
    item_status: DeviantArtStatus option
}

type DeviantArtCommentSiblingsPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    thread: DeviantArtComment list
    context: DeviantArtCommentSiblingsContext
} with
    interface IDeviantArtResultPage<PagingOffset, DeviantArtComment> with
        member this.Cursor =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.thread |> Seq.ofList