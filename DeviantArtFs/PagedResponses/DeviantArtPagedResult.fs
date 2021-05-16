namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    error_code: int option
    results: 'a list option
} with
    interface IDeviantArtResultPage<ParameterTypes.PagingOffset, 'a> with
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results |> Option.defaultValue List.empty |> Seq.ofList