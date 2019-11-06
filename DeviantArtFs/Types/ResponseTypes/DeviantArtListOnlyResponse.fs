namespace DeviantArtFs

open FSharp.Json

/// A DeviantArt response that contains only a "results" field. The library
/// will make things simpler for these calls by returning the string directly.
type DeviantArtListOnlyResponse<'a> = {
    results: 'a list
} with
    /// Parse a DeviantArt "results" response from JSON and return the list as
    /// an IEnumerable<T>.
    static member ParseSeq json =
        let obj = Json.deserialize<DeviantArtListOnlyResponse<'a>> json
        obj.results :> seq<'a>