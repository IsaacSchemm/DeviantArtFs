namespace DeviantArtFs

open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtPostsPagedResult = {
    has_more: bool
    next_offset: int option
    error_code: int option
    results: DeviantArtPost list
} with
    static member Parse json = Json.deserialize<DeviantArtPostsPagedResult> json
    interface IResultPage<int, DeviantArtPost> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList