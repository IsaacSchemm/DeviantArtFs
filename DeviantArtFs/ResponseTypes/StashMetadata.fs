namespace DeviantArtFs.ResponseTypes

open System
open FSharp.Json

type StashSubmission = {
    file_size: string option
    resolution: string option
    submitted_with: SubmittedWith option
}

type StashStats = {
    views: int option
    views_today: int option
    downloads: int option
    downloads_today: int option
}

type StashMetadata = {
    title: string
    path: string option
    size: int option
    description: string option
    parentid: int64 option
    thumb: Preview option
    artist_comments: string option
    original_url: string option
    category: string option
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    creation_time: DateTimeOffset option
    files: Preview list option
    submission: StashSubmission option
    stats: StashStats option
    camera: Map<string, string> option
    stackid: int64 option
    itemid: int64 option
    tags: string list option
}