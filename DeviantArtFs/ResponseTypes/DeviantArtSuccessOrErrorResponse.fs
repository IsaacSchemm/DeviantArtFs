namespace DeviantArtFs

open FSharp.Json

/// A DeviantArt response object with a success flag and (if false) an error description.
type DeviantArtSuccessOrErrorResponse = {
    success: bool
    error_description: string option
} with
    /// Parse a DeviantArt "success" response object from JSON.
    static member Parse json = Json.deserialize<DeviantArtSuccessOrErrorResponse> json