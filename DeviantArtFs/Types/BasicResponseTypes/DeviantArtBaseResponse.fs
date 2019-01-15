namespace DeviantArtFs

open FSharp.Json

type DeviantArtBaseResponse = {
    Status: string
    Error: string option
    ErrorDescription: string option
} with
    static member Parse json = Json.deserialize<DeviantArtBaseResponse> json