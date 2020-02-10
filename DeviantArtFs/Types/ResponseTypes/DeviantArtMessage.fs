namespace DeviantArtFs

open System

type DeviantArtMessageSubjectObject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtFolder option
    gallery: DeviantArtFolder option
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
    collection: DeviantArtFolder option
} with
    member this.GetTimestamp() = OptUtils.timeDefault this.ts
    member this.GetOriginator() = OptUtils.recordDefault this.originator
    member this.GetSubjects() = seq {
        match this.subject with
        | None -> ()
        | Some s ->
            yield! OptUtils.toObjSeq s.profile
            yield! OptUtils.toObjSeq s.deviation
            yield! OptUtils.toObjSeq s.status
            yield! OptUtils.toObjSeq s.comment
            yield! OptUtils.toObjSeq s.collection
            yield! OptUtils.toObjSeq s.gallery
    }

    member this.GetStackId() = OptUtils.stringDefault this.stackid
    member this.GetStackCount() = OptUtils.intDefault this.stack_count

    member this.GetHtml() = OptUtils.recordDefault this.html
    member this.GetProfile() = OptUtils.recordDefault this.profile
    member this.GetDeviation() = OptUtils.recordDefault this.deviation
    member this.GetStatus() = OptUtils.recordDefault this.status
    member this.GetComment() = OptUtils.recordDefault this.comment
    member this.GetCollection() = OptUtils.recordDefault this.collection