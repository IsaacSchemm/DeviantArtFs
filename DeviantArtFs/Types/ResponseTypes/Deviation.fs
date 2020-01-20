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
    member this.ToUnion() =
        match this.is_deleted with
        | true -> Deleted
        | false -> Existing {
            deviationid = this.deviationid
            printid = this.printid
            url = this.url.Value
            title = this.title.Value
            category = this.category.Value
            category_path = this.category_path.Value
            is_favourited = this.is_favourited.Value
            author = this.author.Value
            stats = this.stats.Value
            published_time = this.published_time.Value
            allows_comments = this.allows_comments.Value
            preview = this.preview
            content = this.content
            thumbs = this.thumbs.Value
            videos = this.videos
            flash = this.flash
            daily_deviation = this.daily_deviation
            excerpt = this.excerpt
            is_mature = this.is_mature.Value
            is_downloadable = this.is_downloadable.Value
            download_filesize = this.download_filesize
        }

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
}