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
    member this.GetTimestampOrNull() = Option.toNullable this.ts
    member this.GetOriginators() = OptUtils.toSeq this.originator
    member this.GetSubjects() =
        this.subject
        |> Option.map (fun s -> s.Enumerate())
        |> Option.defaultValue Seq.empty

    member this.GetStackIdOrNull() = Option.toObj this.stackid
    member this.GetStackCountOrNull() = Option.toNullable this.stack_count

    member this.GetHtmls() = OptUtils.toSeq this.html
    member this.GetProfiles() = OptUtils.toSeq this.profile
    member this.GetDeviations() = OptUtils.toSeq this.deviation
    member this.GetStatuses() = OptUtils.toSeq this.status
    member this.GetComments() = OptUtils.toSeq this.comment
    member this.GetCollections() = OptUtils.toSeq this.collection