namespace DeviantArtFs

open System
open FSharp.Json

type StashPublishResponse = {
    status: string
    url: string
    deviationid: Guid
} with
    static member Parse json = Json.deserialize<StashPublishResponse> json