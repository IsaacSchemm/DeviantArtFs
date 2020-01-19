﻿namespace DeviantArtFs

open System
open FSharp.Json

type StashMetadata = {
    title: string
    path: string option
    size: int option
    description: string option
    parentid: int64 option
    thumb: DeviationPreview option
    artist_comments: string option
    original_url: string option
    category: string option
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    creation_time: DateTimeOffset option
    files: DeviationPreview list option
    submission: StashSubmission option
    stats: StashStats option
    camera: Map<string, string> option
    stackid: int64 option
    itemid: int64 option
    tags: string list option
} with
    static member Parse json = Json.deserialize<StashMetadata> json

    member this.GetArtistComments() = this.artist_comments |> OptUtils.stringDefault
    member this.GetCamera() = this.camera |> Option.defaultValue Map.empty
    member this.GetCategory() = this.category |> OptUtils.stringDefault
    member this.GetCreationTime() = this.creation_time |> OptUtils.toNullable
    member this.GetDescription() = this.description |> OptUtils.stringDefault
    member this.GetFiles() = this.files |> Option.defaultValue List.empty
    member this.GetItemId() = this.itemid |> OptUtils.toNullable
    member this.GetOriginalUrl() = this.original_url |> OptUtils.stringDefault
    member this.GetParentId() = this.parentid |> OptUtils.toNullable
    member this.GetPath() = this.path |> OptUtils.stringDefault
    member this.GetSize() = this.size |> OptUtils.toNullable
    member this.GetStackId() = this.stackid |> OptUtils.toNullable
    member this.GetStats() = this.stats |> OptUtils.toSeq
    member this.GetSubmissions() = this.submission |> OptUtils.toSeq
    member this.GetTags() = this.tags |> Option.defaultValue List.empty
    member this.GetThumbs() = this.thumb |> OptUtils.toSeq