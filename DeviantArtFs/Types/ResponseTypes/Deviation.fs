namespace DeviantArtFs

open System
open FSharp.Json
open DeviantArtFs.Json.Transforms

[<AllowNullLiteral>]
type IBclDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

[<AllowNullLiteral>]
type IBclDeviationVideo =
    abstract member Src: string
    abstract member Quality: string
    abstract member Filesize: int
    abstract member Duration: int
    
[<AllowNullLiteral>]
type IBclDeviantArtFlash =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    
[<AllowNullLiteral>]
type IBclDailyDeviation =
    abstract member Body: string
    abstract member Time: DateTimeOffset
    abstract member Giver: IBclDeviantArtUser
    abstract member Suggester: IBclDeviantArtUser

[<AllowNullLiteral>]
type IBclDeviation =
    abstract member Deviationid: Guid
    abstract member Printid: Nullable<Guid>
    abstract member Url: string
    abstract member Title: string
    abstract member Category: string
    abstract member CategoryPath: string
    abstract member IsFavourited: bool
    abstract member IsDeleted: bool
    abstract member Author: IBclDeviantArtUser
    abstract member Stats: IBclDeviationStats
    abstract member PublishedTime: Nullable<DateTimeOffset>
    abstract member AllowsComments: bool
    abstract member Preview: IBclDeviationPreview
    abstract member Content: IBclDeviationContent
    abstract member Thumbs: seq<IBclDeviationPreview>
    abstract member Videos: seq<IBclDeviationVideo>
    abstract member Flash: IBclDeviantArtFlash
    abstract member DailyDeviation: IBclDailyDeviation
    abstract member Excerpt: string
    abstract member IsMature: bool

type DeviationStats = {
    comments: int
    favourites: int
} with
    interface IBclDeviationStats with
        member this.Comments = this.comments
        member this.Favourites = this.favourites

type DeviationVideo = {
    src: string
    quality: string
    filesize: int
    duration: int
} with
    interface IBclDeviationVideo with
        member this.Duration = this.duration
        member this.Filesize = this.filesize
        member this.Quality = this.quality
        member this.Src = this.src

type DeviationFlash = {
    src: string
    height: int
    width: int
} with
    interface IBclDeviantArtFlash with
        member this.Height = this.height
        member this.Src = this.src
        member this.Width = this.width

type DailyDeviation = {
    body: string
    time: DateTimeOffset
    giver: DeviantArtUser
    suggester: DeviantArtUser option
} with
    interface IBclDailyDeviation with
        member this.Body = this.body
        member this.Giver = this.giver :> IBclDeviantArtUser
        member this.Suggester = this.suggester |> Option.map (fun o -> o :> IBclDeviantArtUser) |> Option.toObj
        member this.Time = this.time

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
    thumbs: DeviationPreview[]
    videos: DeviationVideo[] option
    flash: DeviationFlash option
    daily_deviation: DailyDeviation option
    excerpt: string option
    is_mature: bool option
    is_downloadable: bool option
    download_filesize: int option
} with
    static member Parse json = Json.deserialize<Deviation> json
    interface IBclDeviation with
        member this.AllowsComments = this.allows_comments |> Option.defaultValue false
        member this.Author = this.author |> Option.map (fun u -> u :> IBclDeviantArtUser) |> Option.toObj
        member this.Category = this.category |> Option.toObj
        member this.CategoryPath = this.category_path |> Option.toObj
        member this.Content = this.content |> Option.map (fun u -> u :> IBclDeviationContent) |> Option.toObj
        member this.DailyDeviation = this.daily_deviation |> Option.map (fun o -> o :> IBclDailyDeviation) |> Option.toObj
        member this.Deviationid = this.deviationid
        member this.Excerpt = this.excerpt |> Option.toObj
        member this.Flash = this.flash |> Option.map (fun u -> u :> IBclDeviantArtFlash) |> Option.toObj
        member this.IsDeleted = this.is_deleted
        member this.IsFavourited = this.is_favourited |> Option.defaultValue false
        member this.IsMature = this.is_mature |> Option.defaultValue false
        member this.Preview = this.preview |> Option.map (fun u -> u :> IBclDeviationPreview) |> Option.toObj
        member this.Printid = this.printid |> Option.toNullable
        member this.PublishedTime = this.published_time |> Option.toNullable
        member this.Stats = this.stats |> Option.map (fun u -> u :> IBclDeviationStats) |> Option.toObj
        member this.Thumbs = this.thumbs |> Seq.map (fun u -> u :> IBclDeviationPreview)
        member this.Title = this.title |> Option.toObj
        member this.Url = this.url |> Option.toObj
        member this.Videos = this.videos |> Option.map Seq.ofArray |> Option.defaultValue Seq.empty |> Seq.map (fun o -> o :> IBclDeviationVideo)