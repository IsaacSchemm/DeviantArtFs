namespace DeviantArtFs

open FSharp.Json

type TextOnlyResponse = {
    text: string
} with  
    static member Parse json = Json.deserialize<TextOnlyResponse> json