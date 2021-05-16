namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

/// A single page of results from a DeviantArt API endpoint that fetches
/// comments.
type DeviantArtCommentPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    total: int option
    thread: DeviantArtComment list
} with
    interface IDeviantArtResultPage<PagingOffset, DeviantArtComment> with
        member this.Cursor =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.thread |> Seq.ofList