namespace DeviantArtFs.ResponseTypes

open System

type MessageSubject = {
    profile: User option
    deviation: Deviation option
    status: Status option
    comment: Comment option
    collection: CollectionFolder option
    gallery: GalleryFolder option
}

type Message = {
    messageid: string
    ``type``: string
    orphaned: bool
    ts: DateTimeOffset option
    stackid: string option
    stack_count: int option
    is_new: bool
    originator: User option
    subject: MessageSubject option
    html: string option
    profile: User option
    deviation: Deviation option
    status: Status option
    comment: Comment option
    collection: CollectionFolder option
}