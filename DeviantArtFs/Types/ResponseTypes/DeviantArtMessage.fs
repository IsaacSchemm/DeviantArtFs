namespace DeviantArtFs

open System

[<RequireQualifiedAccess>]
type DeviantArtMessageSubject =
| Profile of DeviantArtUser
| Deviation of Deviation
| Status of DeviantArtStatus
| Comment of DeviantArtComment
| Collection of DeviantArtCollectionFolder
| Gallery of DeviantArtGalleryFolder
| Other of obj
| None
with
    member this.GetUnderlyingObject(): obj =
        match this with
        | Profile x -> x :> obj
        | Deviation x -> x :> obj
        | Status x -> x :> obj
        | Comment x -> x :> obj
        | Collection x -> x :> obj
        | Gallery x -> x :> obj
        | Other x -> x
        | None -> null

type DeviantArtMessageSubjectObject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
    gallery: DeviantArtGalleryFolder option
} with
    member this.Discrimate() =
        match (this.profile, this.deviation, this.status, this.comment, this.collection, this.gallery) with
        | (Some x, None, None, None, None, None) -> DeviantArtMessageSubject.Profile x
        | (None, Some x, None, None, None, None) -> DeviantArtMessageSubject.Deviation x
        | (None, None, Some x, None, None, None) -> DeviantArtMessageSubject.Status x
        | (None, None, None, Some x, None, None) -> DeviantArtMessageSubject.Comment x
        | (None, None, None, None, Some x, None) -> DeviantArtMessageSubject.Collection x
        | (None, None, None, None, None, Some x) -> DeviantArtMessageSubject.Gallery x
        | (None, None, None, None, None, None) -> DeviantArtMessageSubject.None
        | _ -> DeviantArtMessageSubject.Other this

type DeviantArtMessage = {
    messageid: string
    ``type``: string
    orphaned: bool

    ts: DateTimeOffset option
    originator: DeviantArtUser option
    subject: DeviantArtMessageSubjectObject option
    
    stackid: string option
    stack_count: int option

    html: string option
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
} with
    member this.GetTimestampOrNull() = this.ts |> Option.toNullable
    member this.GetOriginatorOrEmpty() = this.originator |> Seq.singleton |> Seq.choose id
    member this.GetSubjectOrEmpty() = this.subject |> Seq.singleton |> Seq.choose id