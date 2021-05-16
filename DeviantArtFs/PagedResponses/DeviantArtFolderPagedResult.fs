namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtFolderPagedResult = {
    has_more: bool
    next_offset: int option
    name: string option
    results: Deviation list
} with
    interface IDeviantArtResultPage<PagingOffset, Deviation> with
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results |> Seq.ofList