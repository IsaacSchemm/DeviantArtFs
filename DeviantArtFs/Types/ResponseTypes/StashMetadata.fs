namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclStashMetadata =
    abstract member Json: string
    abstract member Title: string
    abstract member Path: string
    abstract member Size: Nullable<int>
    abstract member Description: string
    abstract member Parentid: Nullable<int64>
    abstract member Thumb: IBclDeviationPreview
    abstract member ArtistComments: string
    abstract member OriginalUrl: string
    abstract member Category: string
    abstract member CreationTime: Nullable<DateTimeOffset>
    abstract member Files: seq<IBclDeviationPreview>
    abstract member Html: string
    abstract member Submission: IBclStashSubmission
    abstract member Stats: IBclStashStats
    abstract member Camera: System.Collections.Generic.IDictionary<string, string>
    abstract member Stackid: Nullable<int64>
    abstract member Itemid: Nullable<int64>
    abstract member Tags: seq<string>

type StashMetadata = {
    title: string
    path: string option
    size: int option
    description: string option
    parentid: int64 option
    thumb: DeviationPreview option
    artist_comments: string option
    original_url: string option
    category: string option
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    creation_time: DateTimeOffset option
    files: DeviationPreview[] option
    submission: StashSubmission option
    stats: StashStats option
    camera: Map<string, string> option
    stackid: int64 option
    itemid: int64 option
    tags: string[] option
    html: string option
} with
    static member Parse json = Json.deserialize<StashMetadata> json
    member this.Json = Json.serialize this
    interface IBclStashMetadata with
        member this.Json = this.Json
        member this.ArtistComments = this.artist_comments |> Option.toObj
        member this.Camera = this.camera |> Option.map (fun o -> o :> System.Collections.Generic.IDictionary<string, string>) |> Option.toObj
        member this.Category = this.category |> Option.toObj
        member this.CreationTime = this.creation_time |> Option.toNullable
        member this.Description = this.description |> Option.toObj
        member this.Files = this.files |> Option.map Seq.ofArray |> Option.defaultValue Seq.empty |> Seq.map (fun o -> o :> IBclDeviationPreview)
        member this.Html = this.html |> Option.toObj
        member this.Itemid = this.itemid |> Option.toNullable
        member this.OriginalUrl = this.original_url |> Option.toObj
        member this.Parentid = this.parentid |> Option.toNullable
        member this.Path = this.path |> Option.toObj
        member this.Size = this.size |> Option.toNullable
        member this.Stackid = this.stackid |> Option.toNullable
        member this.Stats = this.stats |> Option.map (fun o -> o :> IBclStashStats) |> Option.toObj
        member this.Submission = this.submission |> Option.map (fun o -> o :> IBclStashSubmission) |> Option.toObj
        member this.Tags = this.tags |> Option.map Seq.ofArray |> Option.defaultValue Seq.empty
        member this.Thumb = this.thumb |> Option.map (fun o -> o :> IBclDeviationPreview) |> Option.toObj
        member this.Title = this.title