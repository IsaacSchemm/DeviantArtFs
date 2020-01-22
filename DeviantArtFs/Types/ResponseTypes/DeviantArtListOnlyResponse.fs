namespace DeviantArtFs

open FSharp.Json

/// A DeviantArt response that contains only a "results" field.
type DeviantArtListOnlyResponse<'a> = {
    results: 'a list
} with
    /// Parse a DeviantArt "results" response from JSON and return the list as
    /// an FSharpList<T>.
    static member ParseList json =
        let obj = Json.deserialize<DeviantArtListOnlyResponse<'a>> json
        obj.results