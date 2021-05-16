namespace DeviantArtFs.ResponseTypes

open System
open FSharp.Json
open DeviantArtFs.Transforms

type Stats = {
    comments: int
    favourites: int
}

type Video = {
    src: string
    quality: string
    filesize: int
    duration: int
}

type Flash = {
    src: string
    height: int
    width: int
}

type DailyDeviation = {
    body: string
    time: DateTimeOffset
    giver: User
    suggester: User option
}

type SuggestedReason =
| WatchingUser = 1
| SimilarToRecentEngagement = 4

type Deviation = {
    deviationid: Guid
    printid: Guid option
    url: string option
    title: string option
    category: string option
    category_path: string option
    is_favourited: bool option
    is_deleted: bool
    is_published: bool option
    author: User option
    stats: Stats option
    [<JsonField(Transform=typeof<DateTimeOffsetEpochAsString>)>]
    published_time: DateTimeOffset option
    allows_comments: bool option
    preview: Preview option
    content: Content option
    thumbs: Preview list option
    videos: Video list option
    flash: Flash option
    daily_deviation: DailyDeviation option
    excerpt: string option
    is_mature: bool option
    is_downloadable: bool option
    download_filesize: int option
    suggested_reasons: SuggestedReason list option
}