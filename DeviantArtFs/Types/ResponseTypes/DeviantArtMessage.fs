namespace DeviantArtFs

open System

type DeviantArtMessageSubjectObject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtFolder option
    gallery: DeviantArtFolder option
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
    collection: DeviantArtFolder option
}