namespace DeviantArtFs

open System
open FSharp.Json
open System.Collections.Generic

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
    collections: DeviantArtCollectionFolder list option
}

type DeviationMetadataResponse = {
    metadata: DeviationMetadata list
} with
    static member ParseList json =
        let o = Json.deserialize<DeviationMetadataResponse> json
        o.metadata