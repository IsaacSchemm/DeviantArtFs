namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviationMetadata =
    abstract member Deviationid: Guid
    abstract member Printid: Nullable<Guid>
    abstract member Author: IBclDeviantArtUser
    abstract member IsWatching: bool
    abstract member Title: string
    abstract member Description: string
    abstract member License: string
    abstract member AllowsComments: bool
    abstract member Tags: seq<IBclDeviationTag>
    abstract member IsFavourited: bool
    abstract member IsMature: bool
    abstract member Submission: IBclDeviationMetadataSubmission
    abstract member Stats: IBclDeviationMetadataStats
    abstract member Camera: System.Collections.Generic.IDictionary<string, string>
    abstract member Collections: seq<IBclDeviantArtCollectionFolder>

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
    interface IBclDeviationMetadata with
        member this.AllowsComments = this.allows_comments
        member this.Author = this.author :> IBclDeviantArtUser
        member this.Camera = this.camera |> Option.map (fun o -> o :> System.Collections.Generic.IDictionary<string, string>) |> Option.toObj
        member this.Collections = this.collections |> Option.map (Seq.map (fun f -> f :> IBclDeviantArtCollectionFolder)) |> Option.defaultValue Seq.empty
        member this.Description = this.description
        member this.Deviationid = this.deviationid
        member this.IsFavourited = this.is_favourited
        member this.IsMature = this.is_mature
        member this.IsWatching = this.is_watching
        member this.License = this.license
        member this.Printid = this.printid |> Option.toNullable
        member this.Stats = this.stats |> Option.map (fun x -> x :> IBclDeviationMetadataStats) |> Option.toObj
        member this.Submission = this.submission |> Option.map (fun x -> x :> IBclDeviationMetadataSubmission) |> Option.toObj
        member this.Tags = this.tags |> Seq.map (fun t -> t :> IBclDeviationTag)
        member this.Title = this.title

type DeviationMetadataResponse = {
    metadata: DeviationMetadata list
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviationMetadataResponse> json
        o.metadata :> seq<DeviationMetadata>