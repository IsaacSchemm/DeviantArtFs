namespace DeviantArtFs

open FSharp.Json

type DeviantArtTextOnlyResponse = {
    text: string
} with  
    static member Parse json = Json.deserialize<DeviantArtTextOnlyResponse> json