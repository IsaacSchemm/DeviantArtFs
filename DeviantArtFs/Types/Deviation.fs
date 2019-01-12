namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

[<AllowNullLiteral>]
type IBclDeviation =
    abstract member Deviationid: Guid
    abstract member IsDeleted: bool
    abstract member Url: string
    abstract member Title: string
    abstract member Category: string
    abstract member CategoryPath: string
    abstract member IsFavourited: bool
    abstract member Author: IDeviantArtUser
    abstract member Stats: IDeviationStats
    abstract member PublishedTime: Nullable<DateTimeOffset>
    abstract member IsMature: bool
    abstract member AllowsComments: bool
    abstract member Excerpt: string
    abstract member Preview: IDeviationPreview
    abstract member Content: IDeviationContent
    abstract member Thumbs: seq<IDeviationPreview>

type Deviation(original: DeviationResponse.Root) =
    let convertUser (u: DeviationResponse.Author) = {
        new IDeviantArtUser with
            member __.Userid = u.Userid
            member __.Username = u.Username
            member __.Usericon = u.Usericon
            member __.Type = u.Type
    }

    let convertStats (s: DeviationResponse.Stats) = {
        new IDeviationStats with
            member __.Comments = s.Comments
            member __.Favourites = s.Favourites
    }

    let convertPreview (o: DeviationResponse.Thumb) = {
        new IDeviationPreview with
            member __.Src = o.Src
            member __.Height = o.Height
            member __.Width = o.Width
            member __.Transparency = o.Transparency
    }

    let convertContent (o: DeviationResponse.Content) = {
        new IDeviationContent with
            member __.Src = o.Src
            member __.Height = o.Height
            member __.Width = o.Width
            member __.Transparency = o.Transparency
            member __.Filesize = o.Filesize
    }

    let epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)

    let orNull = Option.toObj
    let orFalse = Option.defaultValue false

    member __.Original = original
    member __.Deviationid = original.Deviationid
    member __.IsDeleted = original.IsDeleted
    member __.Url = original.Url
    member __.Title = original.Title
    member __.Category = original.Category
    member __.CategoryPath = original.CategoryPath
    member __.IsFavourited = original.IsFavourited
    member __.Author = original.Author |> Option.map convertUser
    member __.Stats = original.Stats |> Option.map convertStats
    member __.PublishedTime = original.PublishedTime |> Option.map (float >> epoch.AddSeconds)
    member __.IsMature = original.IsMature
    member __.AllowsComments = original.AllowsComments
    member __.Excerpt = original.Excerpt
    member __.Preview = original.Preview |> Option.map convertPreview
    member __.Content = original.Content |> Option.map convertContent
    member __.Thumbs = original.Thumbs |> Seq.map convertPreview

    interface IBclDeviation with
        member this.AllowsComments = this.AllowsComments |> orFalse
        member this.Author = this.Author |> orNull
        member this.Category = this.Category |> orNull
        member this.CategoryPath = this.CategoryPath |> orNull
        member this.Content = this.Content |> orNull
        member this.Deviationid = this.Deviationid
        member this.Excerpt = this.Excerpt |> orNull
        member this.IsDeleted = this.IsDeleted
        member this.IsFavourited = this.IsFavourited |> orFalse
        member this.IsMature = this.IsMature |> orFalse
        member this.Preview = this.Preview |> orNull
        member this.PublishedTime = this.PublishedTime |> Option.toNullable
        member this.Stats = this.Stats |> orNull
        member this.Thumbs = this.Thumbs
        member this.Title = this.Title |> orNull
        member this.Url = this.Url |> orNull