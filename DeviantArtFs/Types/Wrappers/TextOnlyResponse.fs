namespace DeviantArtFs

open FSharp.Json

type internal TextOnlyResponse = {
    text: string
} with  
    static member Parse json = Json.deserialize<TextOnlyResponse> json