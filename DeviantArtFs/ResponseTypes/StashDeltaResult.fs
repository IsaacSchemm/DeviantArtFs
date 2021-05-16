namespace DeviantArtFs

type StashDeltaResult = {
    cursor: string
    has_more: bool
    next_offset: int option
    reset: bool
    entries: StashDeltaEntry list
} with
    interface IDeviantArtResultPage<ParameterTypes.PagingOffset, StashDeltaEntry> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.map ParameterTypes.PagingOffset |> Option.defaultValue ParameterTypes.FromStart
        member this.Items = this.entries |> Seq.ofList