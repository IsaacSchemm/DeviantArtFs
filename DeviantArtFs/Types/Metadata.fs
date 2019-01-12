namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IMetadataTag =
    abstract member TagName: string
    abstract member Sponsored: bool
    abstract member Sponsor: string

type MetadataTag = {
    TagName: string
    Sponsored: bool
    Sponsor: string option
} with
    interface IMetadataTag with
        member this.TagName = this.TagName
        member this.Sponsored = this.Sponsored
        member this.Sponsor = this.Sponsor |> Option.toObj

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

type IBclMetadata =
    abstract member Deviationid: Guid
    abstract member Printid: Nullable<Guid>
    abstract member Author: IDeviantArtUser
    abstract member IsWatching: bool
    abstract member Title: string
    abstract member Description: string
    abstract member License: string
    abstract member AllowsComments: bool
    abstract member Tags: seq<IMetadataTag>
    abstract member IsFavourited: bool
    abstract member IsMature: bool
    abstract member Submission: IMetadataSubmission
    abstract member Stats: IMetadataStats
    abstract member Folders: seq<IDeviantArtCollection>

type Metadata(original: MetadataResponse.Metadata) =
    member __.Original = original

    member __.Deviationid = original.Deviationid
    member __.Printid = original.Printid
    member __.Author = {
        Userid = original.Author.Userid
        Username = original.Author.Username
        Usericon = original.Author.Usericon
        Type = original.Author.Type
    }
    member __.IsWatching = original.IsWatching
    member __.Title = original.Title
    member __.Description = original.Description
    member __.License = original.License
    member __.AllowsComments = original.AllowsComments
    member __.Tags = original.Tags |> Seq.map (fun o -> {
        TagName = o.TagName
        Sponsored = o.Sponsored
        Sponsor = o.Sponsor
    })
    member __.IsFavourited = original.IsFavourited
    member __.IsMature = original.IsMature

    member __.Submission = original.Submission |> Option.map (fun s -> {
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
    })
    member __.Stats = original.Stats |> Option.map (fun s -> {
        new IMetadataStats with
            member __.Views = s.Views
            member __.ViewsToday = s.ViewsToday
            member __.Favourites = s.Favourites
            member __.Comments = s.Comments
            member __.Downloads = s.Downloads
            member __.DownloadsToday = s.DownloadsToday
    })
    member __.Folders = original.Collections |> Seq.map (fun f -> {
        new IDeviantArtCollection with
            member __.Folderid = f.Folderid
            member __.Name = f.Name
    })

    interface IBclMetadata with
        member this.AllowsComments = this.AllowsComments
        member this.Author = this.Author :> IDeviantArtUser
        member this.Description = this.Description
        member this.Deviationid = this.Deviationid
        member this.Folders = this.Folders
        member this.IsFavourited = this.IsFavourited
        member this.IsMature = this.IsMature
        member this.IsWatching = this.IsWatching
        member this.License = this.License
        member this.Printid = this.Printid |> Option.toNullable
        member this.Stats = this.Stats |> Option.toObj
        member this.Submission = this.Submission |> Option.toObj
        member this.Tags = this.Tags |> Seq.map (fun t -> t :> IMetadataTag)
        member this.Title = this.Title