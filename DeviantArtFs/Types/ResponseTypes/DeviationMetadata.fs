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
} with
    member this.GetPrintId() = OptUtils.toNullable this.printid
    member this.GetSubmission() = OptUtils.toSeq this.submission
    member this.GetStats() = OptUtils.toSeq this.stats
    member this.GetCamera() = OptUtils.mapDefault this.camera
    member this.GetCollections() = OptUtils.listDefault this.collections

type DeviationMetadataResponse = {
    metadata: DeviationMetadata list
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviationMetadataResponse> json
        o.metadata :> seq<DeviationMetadata>