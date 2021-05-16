namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

type StashDeltaResult = {
    cursor: string
    has_more: bool
    next_offset: int option
    reset: bool
    entries: StashDeltaEntry list
} with
    interface IDeviantArtResultPage<PagingOffset, StashDeltaEntry> with
        member this.Cursor =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.entries |> Seq.ofList