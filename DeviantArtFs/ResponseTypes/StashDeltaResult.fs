namespace DeviantArtFs

type StashDeltaResult = {
    cursor: string
    has_more: bool
    next_offset: int option
    reset: bool
    entries: StashDeltaEntry list
} with
    interface IResultPage<DeviantArtPagingParams, StashDeltaEntry> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0 |> DeviantArtPagingParams.MaxFrom
        member this.Items = this.entries |> Seq.ofList