namespace DeviantArtFs

open FSharp.Json

/// A DeviantArt response that contains only a "text" field.
type DeviantArtTextOnlyResponse = {
    text: string
} with
    /// Parse a DeviantArt "text" response from JSON and return the text as
    /// a string.
    static member ParseString json =
        let obj = Json.deserialize<DeviantArtTextOnlyResponse> json
        obj.text