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