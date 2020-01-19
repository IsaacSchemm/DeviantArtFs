namespace DeviantArtFs

open System
open FSharp.Json

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
} with
    static member Parse json = Json.deserialize<DeviantArtNote> json
    member this.GetRawBody() = OptUtils.stringDefault this.raw_body