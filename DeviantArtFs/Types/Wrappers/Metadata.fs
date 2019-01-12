namespace DeviantArtFs

open System

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
        new IDeviantArtUser with
            member __.Userid = original.Author.Userid
            member __.Username = original.Author.Username
            member __.Usericon = original.Author.Usericon
            member __.Type = original.Author.Type
    }
    member __.IsWatching = original.IsWatching
    member __.Title = original.Title
    member __.Description = original.Description
    member __.License = original.License
    member __.AllowsComments = original.AllowsComments
    member __.Tags = original.Tags |> Seq.map MetadataTag
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
        member this.Author = this.Author
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