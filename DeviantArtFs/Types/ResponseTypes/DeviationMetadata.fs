namespace DeviantArtFs

open System
open FSharp.Json

type DeviationMetadata = {
    deviationid: Guid
    printid: Guid option
    author: DeviantArtUser
    is_watching: bool
    title: string
    description: string
    license: string
    allows_comments: bool
    tags: DeviationTag list
    is_favourited: bool
    is_mature: bool
    submission: DeviationMetadataSubmission option
    stats: DeviationMetadataStats option
    camera: Map<string, string> option
    collections: DeviantArtFolder list option
}

type DeviationMetadataResponse = {
    metadata: DeviationMetadata list
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviationMetadataResponse> json
        o.metadata :> seq<DeviationMetadata>