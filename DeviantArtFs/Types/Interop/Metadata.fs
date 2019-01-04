namespace DeviantArtFs.Interop

open System
open DeviantArtFs

[<AllowNullLiteral>]
type IMetadataTag =
    abstract member TagName: string
    abstract member Sponsored: bool
    abstract member Sponsor: string

[<AllowNullLiteral>]
type IMetadataSubmittedWith =
    abstract member App: string
    abstract member Url: string

[<AllowNullLiteral>]
type IMetadataSubmission =
    abstract member CreationTime: DateTimeOffset
    abstract member Category: string
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IMetadataSubmittedWith

[<AllowNullLiteral>]
type IMetadataStats =
    abstract member Views: int
    abstract member ViewsToday: int
    abstract member Favourites: int
    abstract member Comments: int
    abstract member Downloads: int
    abstract member DownloadsToday: int

type Metadata(original: MetadataResponse.Metadata) =
    let convertUser (u: MetadataResponse.Author) = {
        new IDeviantArtUser with
            member __.Userid = u.Userid
            member __.Username = u.Username
            member __.Usericon = u.Usericon
            member __.Type = u.Type
    }

    let convertTag (t: MetadataResponse.Tag) = {
        new IMetadataTag with
            member __.TagName = t.TagName
            member __.Sponsored = t.Sponsored
            member __.Sponsor = t.Sponsor |> Option.toObj
    }

    let convertSubmission (s: MetadataResponse.Submission) = {
        new IMetadataSubmission with
            member __.CreationTime = s.CreationTime
            member __.Category = s.Category
            member __.FileSize = s.FileSize
            member __.Resolution = s.Resolution
            member __.SubmittedWith = {
                new IMetadataSubmittedWith with
                    member __.App = s.SubmittedWith.App
                    member __.Url = s.SubmittedWith.Url
            }
    }

    let convertStats (s: MetadataResponse.Stats) = {
        new IMetadataStats with
            member __.Views = s.Views
            member __.ViewsToday = s.ViewsToday
            member __.Favourites = s.Favourites
            member __.Comments = s.Comments
            member __.Downloads = s.Downloads
            member __.DownloadsToday = s.DownloadsToday
    }

    let convertCollection (f: MetadataResponse.Collection) = {
        new IDeviantArtCollection with
            member __.Folderid = f.Folderid
            member __.Name = f.Name
    }

    let map = Option.map
    let orNull = Option.toObj
    let orNullable = Option.toNullable

    member __.Original = original

    member __.Deviationid = original.Deviationid
    member __.Printid = original.Printid |> orNullable
    member __.Author = original.Author |> convertUser
    member __.IsWatching = original.IsWatching
    member __.Title = original.Title
    member __.Description = original.Description
    member __.License = original.License
    member __.AllowsComments = original.AllowsComments
    member __.Tags = original.Tags |> Seq.map convertTag
    member __.IsFavourited = original.IsFavourited
    member __.IsMature = original.IsMature

    member __.Submission = original.Submission |> map convertSubmission |> orNull
    member __.Stats = original.Stats |> map convertStats |> orNull
    member __.Folders = original.Collections |> Seq.map convertCollection