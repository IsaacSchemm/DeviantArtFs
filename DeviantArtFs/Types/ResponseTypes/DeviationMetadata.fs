namespace DeviantArtFs

open System
open FSharp.Json
open System.Collections.Generic

type IBclDeviationMetadata =
    /// The deviation's ID in the DeviantArt API.
    abstract member Deviationid: Guid
    /// The UUID of the print. Available if the author chooses the "Sell Prints" option during submission.
    abstract member Printid: Nullable<Guid>
    /// Information about the user who posted the deviation.
    abstract member Author: IBclDeviantArtUser
    /// Whether the logged-in user is watching the author of this deviation.
    abstract member IsWatching: bool
    /// The deviation's title. May be null.
    abstract member Title: string
    /// An HTML description of the deviation.
    abstract member Description: string
    /// A text description of this deviation's license.
    abstract member License: string
    /// Whether the author has allowed comments.
    abstract member AllowsComments: bool
    /// A list of tags on this deviation.
    abstract member Tags: seq<IBclDeviationTag>
    /// Whether the logged-in user has added this deviation to their favorites.
    abstract member IsFavourited: bool
    /// Whether the deviation contains mature content.
    abstract member IsMature: bool
    /// Submission information, including creation time and file size. May be null.
    abstract member Submission: IBclDeviationMetadataSubmission
    /// Statistics, such as views and comments. May be null.
    abstract member Stats: IBclDeviationMetadataStats
    /// EXIF information from the camera. May be empty.
    abstract member Camera: IDictionary<string, string>
    /// A list of which of the logged-in user's collections this deviation belongs to. May be empty.
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
        member this.Camera = this.camera |> Option.defaultValue Map.empty :> IDictionary<string, string>
        member this.Collections = this.collections |> Option.defaultValue List.empty |> Seq.map (fun f -> f :> IBclDeviantArtCollectionFolder)
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