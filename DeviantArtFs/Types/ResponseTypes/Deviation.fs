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
    /// The deviation's ID in the DeviantArt API.
    abstract member Deviationid: Guid
    /// The UUID of the print. Available if the author chooses the "Sell Prints" option during submission.
    abstract member Printid: Nullable<Guid>
    /// The deviation's URL. May be null.
    abstract member Url: string
    /// The deviation's title. May be null.
    abstract member Title: string
    /// The name of the deviation's category. May be null.
    abstract member Category: string
    /// The path of the deviation's category (e.g. "digitalart/paintings/other"). May be null.
    abstract member CategoryPath: string
    /// Whether the logged-in user has added this deviation to their favorites.
    abstract member IsFavourited: Nullable<bool>
    /// Whether this deviation has been deleted.
    abstract member IsDeleted: bool
    /// Information about the user who posted the deviation. May be null.
    abstract member Author: IBclDeviantArtUser
    /// Statistics about the deviation. May be null.
    abstract member Stats: IBclDeviationStats
    /// The date and time at which the deviation was posted.
    abstract member PublishedTime: Nullable<DateTimeOffset>
    /// Whether the author has allowed comments.
    abstract member AllowsComments: Nullable<bool>
    /// A preview image of the deviation. May be null.
    abstract member Preview: IBclDeviationPreview
    /// The content image of the deviation. May be null.
    abstract member Content: IBclDeviationContent
    /// A list of thumbnails. May be empty.
    abstract member Thumbs: seq<IBclDeviationPreview>
    /// A list of videos included in the deviation. May be empty.
    abstract member Videos: seq<IBclDeviationVideo>
    /// Information about the Flash object in the deviation, if any. May be null.
    abstract member Flash: IBclDeviantArtFlash
    /// Information about when this deviation got a Daily Deviation award, if ever. May be null.
    abstract member DailyDeviation: IBclDailyDeviation
    /// An HTML excerpt (for literature submissions). May be null.
    abstract member Excerpt: string
    /// Whether the deviation contains mature content.
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
    interface IBclDeviation with
        member this.AllowsComments = this.allows_comments |> Option.toNullable
        member this.Author = this.author |> Option.map (fun u -> u :> IBclDeviantArtUser) |> Option.toObj
        member this.Category = this.category |> Option.toObj
        member this.CategoryPath = this.category_path |> Option.toObj
        member this.Content = this.content |> Option.map (fun u -> u :> IBclDeviationContent) |> Option.toObj
        member this.DailyDeviation = this.daily_deviation |> Option.map (fun o -> o :> IBclDailyDeviation) |> Option.toObj
        member this.Deviationid = this.deviationid
        member this.Excerpt = this.excerpt |> Option.toObj
        member this.Flash = this.flash |> Option.map (fun u -> u :> IBclDeviantArtFlash) |> Option.toObj
        member this.IsDeleted = this.is_deleted
        member this.IsFavourited = this.is_favourited |> Option.toNullable
        member this.IsMature = this.is_mature |> Option.defaultValue false
        member this.Preview = this.preview |> Option.map (fun u -> u :> IBclDeviationPreview) |> Option.toObj
        member this.Printid = this.printid |> Option.toNullable
        member this.PublishedTime = this.published_time |> Option.toNullable
        member this.Stats = this.stats |> Option.map (fun u -> u :> IBclDeviationStats) |> Option.toObj
        member this.Thumbs = this.thumbs |> Option.defaultValue List.empty |> Seq.map (fun u -> u :> IBclDeviationPreview)
        member this.Title = this.title |> Option.toObj
        member this.Url = this.url |> Option.toObj
        member this.Videos = this.videos |> Option.defaultValue List.empty |> Seq.map (fun o -> o :> IBclDeviationVideo)