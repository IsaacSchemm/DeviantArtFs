namespace DeviantArtFs

open System

type DeviantArtNote = {
    noteid: Guid
    ts: DateTimeOffset
    unread: bool
    starred: bool
    sent: bool
    subject: string
    preview: string
    raw_body: string option
    body: string
    user: DeviantArtUser
    recipients: DeviantArtUser list
}