namespace DeviantArtFs

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

    member this.GetArtistCommentsOrNull() = this.artist_comments |> Option.toObj
    member this.GetCamera() = this.camera |> Option.defaultValue Map.empty
    member this.GetCategoryOrNull() = this.category |> Option.toObj
    member this.GetCreationTimeOrNull() = this.creation_time |> Option.toNullable
    member this.GetDescriptionOrNull() = this.description |> Option.toObj
    member this.GetFiles() = this.files |> Option.defaultValue List.empty
    member this.GetItemIdOrNull() = this.itemid |> Option.toNullable
    member this.GetOriginalUrlOrNull() = this.original_url |> Option.toObj
    member this.GetParentIdOrNull() = this.parentid |> Option.toNullable
    member this.GetPathOrNull() = this.path |> Option.toObj
    member this.GetSizeOrNull() = this.size |> Option.toNullable
    member this.GetStackIdOrNull() = this.stackid |> Option.toNullable
    member this.GetStats() = this.stats |> OptUtils.toSeq
    member this.GetSubmissions() = this.submission |> OptUtils.toSeq
    member this.GetTags() = this.tags |> Option.defaultValue List.empty
    member this.GetThumbs() = this.thumb |> OptUtils.toSeq