namespace DeviantArtFs

open System
open FSharp.Json
open System.Collections.Generic

[<AllowNullLiteral>]
type IBclStashMetadata =
    /// The title of the stack or item.
    abstract member Title: string
    /// The path to the stack or item (separated by forward slashes).
    abstract member Path: string
    /// The number of items in the stack. May be null.
    abstract member Size: Nullable<int>
    /// An HTML description for the stack or item. May be null.
    abstract member Description: string
    /// The ID of the parent stack, if this object represents a child stack.
    abstract member Parentid: Nullable<int64>
    /// A thumbnail for the item or stack. May be null.
    abstract member Thumb: IBclDeviationPreview
    /// HTML artist comments for the item. May be null.
    abstract member ArtistComments: string
    /// The original URL of the item. May be null.
    abstract member OriginalUrl: string
    /// The category path for the item. May be null.
    abstract member Category: string
    /// The creation time of the item. May be null.
    abstract member CreationTime: Nullable<DateTimeOffset>
    /// A list of files for this item. May be empty.
    abstract member Files: seq<IBclDeviationPreview>
    /// Information about the submission, such as file size. May be null.
    abstract member Submission: IBclStashSubmission
    /// Stats about the submission, such as number of views and downloads. May be null.
    abstract member Stats: IBclStashStats
    /// EXIF information from the camera. May be empty.
    abstract member Camera: IDictionary<string, string>
    /// The ID of this stack, or - if this object represents an item - the item's parent stack.
    abstract member Stackid: Nullable<int64>
    /// The item ID, if this object represents an item.
    abstract member Itemid: Nullable<int64>
    /// A list of tags for the submission. May be empty.
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
    files: DeviationPreview list option
    submission: StashSubmission option
    stats: StashStats option
    camera: Map<string, string> option
    stackid: int64 option
    itemid: int64 option
    tags: string list option
} with
    static member Parse json = Json.deserialize<StashMetadata> json

    interface IBclStashMetadata with
        member this.ArtistComments = this.artist_comments |> Option.toObj
        member this.Camera = this.camera |> Option.defaultValue Map.empty :> IDictionary<string, string>
        member this.Category = this.category |> Option.toObj
        member this.CreationTime = this.creation_time |> Option.toNullable
        member this.Description = this.description |> Option.toObj
        member this.Files = this.files |> Option.defaultValue List.empty |> Seq.map (fun o -> o :> IBclDeviationPreview)
        member this.Itemid = this.itemid |> Option.toNullable
        member this.OriginalUrl = this.original_url |> Option.toObj
        member this.Parentid = this.parentid |> Option.toNullable
        member this.Path = this.path |> Option.toObj
        member this.Size = this.size |> Option.toNullable
        member this.Stackid = this.stackid |> Option.toNullable
        member this.Stats = this.stats |> Option.map (fun o -> o :> IBclStashStats) |> Option.toObj
        member this.Submission = this.submission |> Option.map (fun o -> o :> IBclStashSubmission) |> Option.toObj
        member this.Tags = this.tags |> Option.map Seq.ofList |> Option.defaultValue Seq.empty
        member this.Thumb = this.thumb |> Option.map (fun o -> o :> IBclDeviationPreview) |> Option.toObj
        member this.Title = this.title