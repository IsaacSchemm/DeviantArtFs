namespace DeviantArtFs

open System
open FSharp.Json
open DeviantArtFs.Json.Transforms
open System.Runtime.CompilerServices

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
} with
    member this.GetSuggester() = OptUtils.recordDefault this.suggester

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

and DeviationUnion =
| Deleted
| Existing of ExistingDeviation

and ExistingDeviation = {
    deviationid: Guid
    printid: Guid option
    url: string
    title: string
    category: string
    category_path: string
    is_favourited: bool
    author: DeviantArtUser
    stats: DeviationStats
    [<JsonField(Transform=typeof<DateTimeOffsetEpochAsString>)>]
    published_time: DateTimeOffset
    allows_comments: bool
    preview: DeviationPreview option
    content: DeviationContent option
    thumbs: DeviationPreview list
    videos: DeviationVideo list option
    flash: DeviationFlash option
    daily_deviation: DailyDeviation option
    excerpt: string option
    is_mature: bool
    is_downloadable: bool
    download_filesize: int option
} with
    member this.GetPrintId() = OptUtils.guidDefault this.printid
    member this.GetPreview() = OptUtils.recordDefault this.preview
    member this.GetContent() = OptUtils.recordDefault this.content
    member this.GetVideos() = OptUtils.listDefault this.videos
    member this.GetFlash() = OptUtils.recordDefault this.videos
    member this.GetDailyDeviation() = OptUtils.recordDefault this.daily_deviation
    member this.GetExcerpt() = OptUtils.stringDefault this.excerpt
    member this.GetDownloadFilesize() = OptUtils.intDefault this.download_filesize

[<Extension>]
module DeviationExtensions =
    [<Extension>]
    let ToUnion (d: Deviation) =
        match d.is_deleted with
        | true -> Deleted
        | false -> Existing {
            deviationid = d.deviationid
            printid = d.printid
            url = d.url.Value
            title = d.title.Value
            category = d.category.Value
            category_path = d.category_path.Value
            is_favourited = d.is_favourited.Value
            author = d.author.Value
            stats = d.stats.Value
            published_time = d.published_time.Value
            allows_comments = d.allows_comments.Value
            preview = d.preview
            content = d.content
            thumbs = d.thumbs.Value
            videos = d.videos
            flash = d.flash
            daily_deviation = d.daily_deviation
            excerpt = d.excerpt
            is_mature = d.is_mature.Value
            is_downloadable = d.is_downloadable.Value
            download_filesize = d.download_filesize
        }

    [<Extension>]
    let WhereNotDeleted (s: Deviation seq) = seq {
        for d in s do
            if not (isNull (d :> obj)) then
                match ToUnion d with
                | Deleted -> ()
                | Existing e -> yield e
    }