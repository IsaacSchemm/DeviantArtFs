namespace DeviantArtFs

open System
open FSharp.Json

type internal ListOnlyResponse<'a> = {
    results: 'a[]
} with
    static member Parse json = Json.deserialize<ListOnlyResponse<'a>> json