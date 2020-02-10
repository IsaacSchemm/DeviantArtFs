namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtFolderPagedResult = {
    has_more: bool
    next_offset: int option
    name: string option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtFolderPagedResult> json
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    member this.GetName() = OptUtils.stringDefault this.name
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList