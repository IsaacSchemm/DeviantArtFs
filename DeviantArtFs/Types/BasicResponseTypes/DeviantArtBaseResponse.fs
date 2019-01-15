namespace DeviantArtFs

open FSharp.Json

type DeviantArtBaseResponse = {
    status: string option
    error: string option
    error_description: string option
} with
    static member Parse json = Json.deserialize<DeviantArtBaseResponse> json