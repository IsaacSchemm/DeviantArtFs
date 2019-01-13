namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IBclStashMetadata =
    abstract member Title: string
    abstract member Path: string
    abstract member Size: Nullable<int>
    abstract member Description: string
    abstract member Parentid: Nullable<int64>
    abstract member Thumb: IDeviationPreview
    abstract member ArtistComments: string
    abstract member OriginalUrl: string
    abstract member Category: string
    abstract member CreationTime: Nullable<DateTimeOffset>
    abstract member Files: seq<IDeviationPreview>
    abstract member Html: string
    abstract member Submission: IBclStashSubmission
    abstract member Stats: IBclStashStats
    abstract member Stackid: Nullable<int64>
    abstract member Itemid: Nullable<int64>
    abstract member Tags: seq<string>

type StashMetadata(original: StashMetadataResponse.Root) =
    let epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)

    member __.Original = original

    member __.Title = original.Title
    member __.Path = original.Path
    member __.Size = original.Size
    member __.Description = original.Description
    member __.Parentid = original.Parentid
    member __.Thumb = original.Thumb |> Option.map (fun f -> {
        new IDeviationPreview with
            member __.Src = f.Src
            member __.Width = f.Width
            member __.Height = f.Height
            member __.Transparency = f.Transparency
    })
    member __.ArtistComments = original.ArtistComments
    member __.OriginalUrl = original.OriginalUrl
    member __.Category = original.Category
    member __.CreationTime = original.CreationTime |> Option.map (float >> epoch.AddSeconds)
    member __.Files = original.Files |> Seq.map (fun f -> {
        new IDeviationPreview with
            member __.Src = f.Src
            member __.Width = f.Width
            member __.Height = f.Height
            member __.Transparency = f.Transparency
    })
    member __.Html = original.Html
    member __.Submission = original.Submission |> Option.map StashSubmission
    member __.Stats = original.Stats |> Option.map StashStats
    member __.Stackid = original.Stackid
    member __.Itemid = original.Itemid
    member __.Tags = original.Tags

    interface IBclStashMetadata with
        member this.ArtistComments = this.ArtistComments |> Option.toObj
        member this.Category = this.Category |> Option.toObj
        member this.CreationTime = this.CreationTime |> Option.toNullable
        member this.Description = this.Description |> Option.toObj
        member this.Files = this.Files
        member this.Html = this.Html |> Option.toObj
        member this.Itemid = this.Itemid |> Option.toNullable
        member this.OriginalUrl = this.OriginalUrl |> Option.toObj
        member this.Parentid = this.Parentid |> Option.toNullable
        member this.Path = this.Path |> Option.toObj
        member this.Size = this.Size |> Option.toNullable
        member this.Stackid = this.Stackid |> Option.toNullable
        member this.Stats = this.Stats |> Option.map (fun o -> o :> IBclStashStats) |> Option.toObj
        member this.Submission = this.Submission |> Option.map (fun o -> o :> IBclStashSubmission) |> Option.toObj
        member this.Tags = this.Tags :> seq<string>
        member this.Thumb = this.Thumb |> Option.toObj
        member this.Title = this.Title |> Option.toObj