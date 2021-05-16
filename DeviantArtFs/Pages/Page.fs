namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes

type Page<'a> = {
    has_more: bool
    next_offset: int option
    error_code: int option
    results: 'a list option
} with
    interface ILinearPage<'a> with
        member this.NextOffset = this.next_offset
        member this.NextPage =
            match (this.next_offset, this.has_more) with
            | (Some offset, true) -> Some (PagingOffset offset)
            | _ -> None
        member this.Items = this.results |> Option.defaultValue List.empty