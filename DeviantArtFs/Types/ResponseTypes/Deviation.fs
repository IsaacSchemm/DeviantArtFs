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
    member this.GetPrintId() = OptUtils.guidDefault this.printid
    member this.GetUrl() = OptUtils.stringDefault this.url
    member this.GetTitle() = OptUtils.stringDefault this.title
    member this.GetCategory() = OptUtils.stringDefault this.category
    member this.GetCategoryPath() = OptUtils.stringDefault this.category_path
    member this.GetIsFavorited() = OptUtils.boolDefault this.is_favourited
    member this.GetAuthor() = OptUtils.recordDefault this.author
    member this.GetStats() = OptUtils.recordDefault this.stats
    member this.GetPublishedTime() = OptUtils.timeDefault this.published_time
    member this.GetAllowsComments() = OptUtils.boolDefault this.allows_comments
    member this.GetPreview() = OptUtils.recordDefault this.preview
    member this.GetContent() = OptUtils.recordDefault this.content
    member this.GetThumbs() = OptUtils.listDefault this.thumbs
    member this.GetVideos() = OptUtils.listDefault this.videos
    member this.GetFlash() = OptUtils.recordDefault this.flash
    member this.GetDailyDeviation() = OptUtils.recordDefault this.daily_deviation
    member this.GetExcerpt() = OptUtils.stringDefault this.excerpt
    member this.GetIsMature() = OptUtils.boolDefault this.is_mature
    member this.GetIsDownloadable() = OptUtils.boolDefault this.is_downloadable
    member this.GetDownloadFilesize() = OptUtils.intDefault this.download_filesize