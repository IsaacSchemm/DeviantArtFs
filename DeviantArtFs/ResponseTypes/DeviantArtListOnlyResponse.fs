namespace DeviantArtFs

open FSharp.Json

/// An interface to use for response types that just contain a list and nothing else.
type internal IDeviantArtListOnlyResponse<'a> =
    abstract member List: 'a list

/// A DeviantArt response that contains only a "results" field.
type DeviantArtListOnlyResponse<'a> = {
    results: 'a list
} with
    interface IDeviantArtListOnlyResponse<'a> with
        member this.List = this.results
    /// Parse a DeviantArt "results" response from JSON and return the list as
    /// an FSharpList<T>.
    static member ParseList json =
        let obj = Json.deserialize<DeviantArtListOnlyResponse<'a>> json
        obj.results