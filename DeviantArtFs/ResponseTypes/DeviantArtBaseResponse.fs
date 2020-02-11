namespace DeviantArtFs

open FSharp.Json

/// A DeviantArt response object with status and error information.
type DeviantArtBaseResponse = {
    status: string option
    error: string option
    error_description: string option
} with
    /// Parse a DeviantArt "status" response object from JSON.
    static member Parse json = Json.deserialize<DeviantArtBaseResponse> json