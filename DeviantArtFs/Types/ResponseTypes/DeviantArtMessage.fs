namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IBclDeviantArtMessageSubject =
    abstract member Profile: IBclDeviantArtUser
    abstract member Deviation: IBclDeviation
    abstract member Status: IBclDeviantArtStatus
    abstract member Comment: IBclDeviantArtComment
    abstract member Collection: IBclDeviantArtCollectionFolder
    abstract member Gallery: IBclDeviantArtGalleryFolder

type DeviantArtMessageSubject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
    gallery: DeviantArtGalleryFolder option
} with
    interface IBclDeviantArtMessageSubject with
        member this.Collection = this.collection |> Option.map (fun o -> o :> IBclDeviantArtCollectionFolder) |> Option.toObj
        member this.Comment = this.comment |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.Deviation = this.deviation |> Option.map (fun o -> o :> IBclDeviation) |> Option.toObj
        member this.Gallery = this.gallery |> Option.map (fun o -> o :> IBclDeviantArtGalleryFolder) |> Option.toObj
        member this.Profile = this.profile |> Option.map (fun o -> o :> IBclDeviantArtUser) |> Option.toObj
        member this.Status = this.status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj

type IBclDeviantArtMessage =
    abstract member Messageid: string
    abstract member Type: string
    abstract member Orphaned: bool
    abstract member Ts: Nullable<DateTimeOffset>
    abstract member Stackid: string
    abstract member StackCount: Nullable<int>
    abstract member Originator: IBclDeviantArtUser
    abstract member Subject: IBclDeviantArtMessageSubject
    abstract member Html: string
    abstract member Profile: IBclDeviantArtUser
    abstract member Deviation: IBclDeviation
    abstract member Status: IBclDeviantArtStatus
    abstract member Comment: IBclDeviantArtComment
    abstract member Collection: IBclDeviantArtCollectionFolder

// https://www.deviantart.com/developers/http/v1/20160316/object/message
type DeviantArtMessage = {
    messageid: string
    ``type``: string
    orphaned: bool
    ts: DateTimeOffset option
    stackid: string option
    stack_count: int option
    originator: DeviantArtUser option
    subject: DeviantArtMessageSubject option
    html: string option
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
} with
    interface IBclDeviantArtMessage with
        member this.Html = this.html |> Option.toObj
        member this.Messageid = this.messageid
        member this.Originator = this.originator |> Option.map (fun o -> o :> IBclDeviantArtUser) |> Option.toObj
        member this.Orphaned = this.orphaned
        member this.StackCount = this.stack_count |> Option.toNullable
        member this.Stackid = this.stackid |> Option.toObj
        member this.Subject = this.subject |> Option.map (fun o -> o :> IBclDeviantArtMessageSubject) |> Option.toObj
        member this.Ts = this.ts |> Option.toNullable
        member this.Type = this.``type``
        member this.Collection = this.collection |> Option.map (fun o -> o :> IBclDeviantArtCollectionFolder) |> Option.toObj
        member this.Comment = this.comment |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.Deviation = this.deviation |> Option.map (fun o -> o :> IBclDeviation) |> Option.toObj
        member this.Profile = this.profile |> Option.map (fun o -> o :> IBclDeviantArtUser) |> Option.toObj
        member this.Status = this.status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj