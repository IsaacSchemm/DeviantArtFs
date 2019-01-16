namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviation =
    abstract member Deviationid: Guid
    abstract member IsDeleted: bool
    abstract member Url: string
    abstract member Title: string
    abstract member Category: string
    abstract member CategoryPath: string
    abstract member IsFavourited: bool
    abstract member Author: IBclDeviantArtUser
    abstract member Stats: IDeviationStats
    abstract member PublishedTime: Nullable<DateTimeOffset>
    abstract member IsMature: bool
    abstract member AllowsComments: bool
    abstract member Excerpt: string
    abstract member Preview: IBclDeviationPreview
    abstract member Content: IBclDeviationContent
    abstract member Thumbs: seq<IBclDeviationPreview>

type Deviation = {
    deviationid: Guid
    is_deleted: bool
    url: string option
    title: string option
    category: string option
    category_path: string option
    is_favourited: bool option
    author: DeviantArtUser option
    stats: DeviationStats option
    [<JsonField(Transform=typeof<DateTimeOffsetEpochAsString>)>]
    published_time: DateTimeOffset option
    is_mature: bool option
    allows_comments: bool option
    excerpt: string option
    preview: DeviationPreview option
    content: DeviationContent option
    thumbs: DeviationPreview[]
} with
    static member Parse json = Json.deserialize<Deviation> json
    interface IBclDeviation with
        member this.AllowsComments = this.allows_comments |> Option.defaultValue false
        member this.Author = this.author |> Option.map (fun u -> u :> IBclDeviantArtUser) |> Option.toObj
        member this.Category = this.category |> Option.toObj
        member this.CategoryPath = this.category_path |> Option.toObj
        member this.Content = this.content |> Option.map (fun u -> u :> IBclDeviationContent) |> Option.toObj
        member this.Deviationid = this.deviationid
        member this.Excerpt = this.excerpt |> Option.toObj
        member this.IsDeleted = this.is_deleted
        member this.IsFavourited = this.is_favourited |> Option.defaultValue false
        member this.IsMature = this.is_mature |> Option.defaultValue false
        member this.Preview = this.preview |> Option.map (fun u -> u :> IBclDeviationPreview) |> Option.toObj
        member this.PublishedTime = this.published_time |> Option.toNullable
        member this.Stats = this.stats |> Option.map (fun u -> u :> IDeviationStats) |> Option.toObj
        member this.Thumbs = this.thumbs |> Seq.map (fun u -> u :> IBclDeviationPreview)
        member this.Title = this.title |> Option.toObj
        member this.Url = this.url |> Option.toObj