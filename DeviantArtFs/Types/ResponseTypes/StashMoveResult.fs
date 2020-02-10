namespace DeviantArtFs

open FSharp.Json

type StashMoveResult = {
    target: StashMetadata
    changes: StashMetadata list
} with
    static member Parse json = Json.deserialize<StashMoveResult> json