namespace DeviantArtFs

open System

type DeviantArtMessageSubjectObject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
    gallery: DeviantArtGalleryFolder option
} with
    member this.Enumerate() = seq {
        yield! OptUtils.toObjSeq this.profile
        yield! OptUtils.toObjSeq this.deviation
        yield! OptUtils.toObjSeq this.status
        yield! OptUtils.toObjSeq this.comment
        yield! OptUtils.toObjSeq this.collection
        yield! OptUtils.toObjSeq this.gallery
    }

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
    member this.GetTimestamp() = OptUtils.toNullable this.ts
    member this.GetOriginator() = OptUtils.recordDefault this.originator
    member this.GetSubjects() =
        this.subject
        |> Option.map (fun s -> s.Enumerate())
        |> Option.defaultValue Seq.empty

    member this.GetStackId() = OptUtils.stringDefault this.stackid
    member this.GetStackCount() = OptUtils.toNullable this.stack_count

    member this.GetHtml() = OptUtils.recordDefault this.html
    member this.GetProfile() = OptUtils.recordDefault this.profile
    member this.GetDeviation() = OptUtils.recordDefault this.deviation
    member this.GetStatus() = OptUtils.recordDefault this.status
    member this.GetComment() = OptUtils.recordDefault this.comment
    member this.GetCollection() = OptUtils.recordDefault this.collection