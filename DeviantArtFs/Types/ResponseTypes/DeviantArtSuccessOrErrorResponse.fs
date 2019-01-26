namespace DeviantArtFs

open FSharp.Json

type DeviantArtSuccessOrErrorResponse = {
    success: bool
    error_description: string option
} with
    static member Parse json = Json.deserialize<DeviantArtSuccessOrErrorResponse> json