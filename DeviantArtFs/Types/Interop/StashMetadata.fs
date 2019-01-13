namespace DeviantArtFs.Interop

open System
open DeviantArtFs
open System.Runtime.CompilerServices

[<AllowNullLiteral>]
type IStashSubmission =
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IMetadataSubmittedWith
    
[<AllowNullLiteral>]
type IStashStats =
    abstract member Views: Nullable<int>
    abstract member ViewsToday: Nullable<int>
    abstract member Downloads: Nullable<int>
    abstract member DownloadsToday: Nullable<int>
    
[<AllowNullLiteral>]
type StashMetadata(original: StashMetadataResponse.Root) =
    let toStashFile (f: StashMetadataResponse.Thumb) =
        {
            new IDeviationPreview with
                member __.Src = f.Src
                member __.Width = f.Width
                member __.Height = f.Height
                member __.Transparency = f.Transparency
        }

    static member Parse (json: string) =
        json
        |> StashMetadataResponse.Parse
        |> StashMetadata

    member __.Original = original

    member __.Title = original.Title |> Option.toObj
    member __.Path = original.Path |> Option.toObj
    member __.Size = original.Size |> Option.toNullable
    member __.Description = original.Description |> Option.toObj
    member __.Parentid = original.Parentid |> Option.toNullable
    member __.Thumb =
        match original.Thumb with
        | Some s -> toStashFile s
        | None -> null
    member __.ArtistComments = original.ArtistComments |> Option.toObj
    member __.OriginalUrl = original.OriginalUrl |> Option.toObj
    member __.Category = original.Category |> Option.toObj
    member __.CreationTime = original.CreationTime |> Option.toNullable
    member __.Files = original.Files |> Seq.map toStashFile
    member __.Html = original.Html |> Option.toObj
    member __.Submission =
        original.Submission
        |> Option.map (fun s -> {
            new IStashSubmission with
                member __.FileSize = s.FileSize |> Option.toObj
                member __.Resolution = s.Resolution |> Option.toObj
                member __.SubmittedWith =
                    s.SubmittedWith
                    |> Option.map (fun w -> {
                        new IMetadataSubmittedWith with
                            member __.App = w.App
                            member __.Url = w.Url
                    })
                    |> Option.toObj
        })
    member __.Stats =
        original.Stats
        |> Option.map (fun s -> {
            new IStashStats with
                member __.Views = s.Views |> Option.toNullable
                member __.ViewsToday = s.ViewsToday |> Option.toNullable
                member __.Downloads = s.Downloads |> Option.toNullable
                member __.DownloadsToday = s.Views |> Option.toNullable
        })
    member __.Stackid = original.Stackid |> Option.toNullable
    member __.Itemid = original.Itemid |> Option.toNullable
    member __.Tags = original.Tags

type IMoveResult =
    abstract member Target: StashMetadata
    abstract member Changes: seq<StashMetadata>
