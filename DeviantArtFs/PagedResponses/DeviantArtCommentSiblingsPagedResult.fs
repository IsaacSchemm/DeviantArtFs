namespace DeviantArtFs

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
    interface IResultPage<DeviantArtPagingParams, DeviantArtComment> with
        member this.Cursor = DeviantArtPagingParams.MaxFrom (this.next_offset |> Option.defaultValue 0)
        member this.HasMore = this.has_more
        member this.Items = this.thread |> Seq.ofList