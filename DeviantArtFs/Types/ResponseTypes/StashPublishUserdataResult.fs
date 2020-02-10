namespace DeviantArtFs

open FSharp.Json

type StashPublishUserdataResult = {
    features: string list
    agreements: string list
} with
    static member Parse json = Json.deserialize<StashPublishUserdataResult> json