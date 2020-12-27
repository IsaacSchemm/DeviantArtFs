namespace DeviantArtFs

open System

type DeviantArtMessageSubject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
    gallery: DeviantArtGalleryFolder option
}

type DeviantArtMessage = {
    messageid: string
    ``type``: string
    orphaned: bool
    ts: DateTimeOffset option
    stackid: string option
    stack_count: int option
    is_new: bool
    originator: DeviantArtUser option
    subject: DeviantArtMessageSubject option
    html: string option
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
}