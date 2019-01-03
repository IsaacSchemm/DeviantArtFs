namespace DeviantArtFs

open System
open DeviantArtFs

[<AllowNullLiteral>]
type IDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

[<AllowNullLiteral>]
type IDeviationFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Transparency: bool
    abstract member Filesize: Nullable<int>

[<AllowNullLiteral>]
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

    let convertThumb (o: DeviationResponse.Thumb) = {
        new IDeviationFile with
            member __.Src = o.Src
            member __.Height = o.Height
            member __.Width = o.Width
            member __.Transparency = o.Transparency
            member __.Filesize = Nullable()
    }

    let convertContent (o: DeviationResponse.Content) = {
        new IDeviationFile with
            member __.Src = o.Src
            member __.Height = o.Height
            member __.Width = o.Width
            member __.Transparency = o.Transparency
            member __.Filesize = Nullable(o.Filesize)
    }

    let epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)

    let map = Option.map
    let orNull = Option.toObj
    let orNullable = Option.toNullable
    let orFalse = Option.defaultValue false

    member __.Original = original
    member __.Deviationid = original.Deviationid
    member __.IsDeleted = original.IsDeleted

    member __.Url = original.Url |> orNull
    member __.Title = original.Title |> orNull
    member __.Category = original.Category |> orNull
    member __.CategoryPath = original.CategoryPath |> orNull
    member __.IsFavourited = original.IsFavourited |> orFalse
    member __.Author = original.Author |> map convertUser |> orNull
    member __.Stats = original.Stats |> map convertStats |> orNull
    member __.PublishedTime = original.PublishedTime |> map float |> map epoch.AddSeconds |> orNullable
    member __.IsMature = original.IsMature |> orFalse
    member __.AllowsComments = original.AllowsComments |> orFalse

    member __.Excerpt = original.Excerpt |> orNull
    member __.Preview = original.Preview |> map convertThumb |> orNull
    member __.Content = original.Content |> map convertContent |> orNull
    member __.Thumbs = original.Thumbs |> Seq.map convertThumb
