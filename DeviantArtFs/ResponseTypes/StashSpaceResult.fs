namespace DeviantArtFs

open FSharp.Json

type StashSpaceResult = {
    available_space: int64
    total_space: int64
} with
    static member Parse json = Json.deserialize<StashSpaceResult> json