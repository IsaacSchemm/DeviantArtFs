namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes

type CommentSiblingsContext = {
    parent: Comment option
    item_profile: User option
    item_deviation: Deviation option
    item_status: Status option
}

type CommentSiblingsPage = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    thread: Comment list
    context: CommentSiblingsContext
} with
    interface ILinearPage<Comment> with
        member this.NextOffset = this.next_offset
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.thread