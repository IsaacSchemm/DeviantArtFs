namespace DeviantArtFs

open System
open FSharp.Json
open DeviantArtFs.Json.Transforms

type DeviationStats = {
    comments: int
    favourites: int
}

type DeviationVideo = {
    src: string
    quality: string
    filesize: int
    duration: int
}

type DeviationFlash = {
    src: string
    height: int
    width: int
}

type DailyDeviation = {
    body: string
    time: DateTimeOffset
    giver: DeviantArtUser
    suggester: DeviantArtUser option
}

type Deviation = {
    deviationid: Guid
    printid: Guid option
    url: string option
    title: string option
    category: string option
    category_path: string option
    is_favourited: bool option
    is_deleted: bool
    author: DeviantArtUser option
    stats: DeviationStats option
    [<JsonField(Transform=typeof<DateTimeOffsetEpochAsString>)>]
    published_time: DateTimeOffset option
    allows_comments: bool option
    preview: DeviationPreview option
    content: DeviationContent option
    thumbs: DeviationPreview list option
    videos: DeviationVideo list option
    flash: DeviationFlash option
    daily_deviation: DailyDeviation option
    excerpt: string option
    is_mature: bool option
    is_downloadable: bool option
    download_filesize: int option
} with
    static member Parse json = Json.deserialize<Deviation> json