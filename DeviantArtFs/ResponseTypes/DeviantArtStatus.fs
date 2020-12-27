namespace DeviantArtFs

open System

type DeviantArtStatusItem = {
    ``type``: string
    status: DeviantArtStatus option
    deviation: Deviation option
}

and DeviantArtStatus = {
    statusid: Guid option
    body: string option
    ts: DateTimeOffset option
    url: string option
    comments_count: int option
    is_share: bool option
    is_deleted: bool
    author: DeviantArtUser option
    items: DeviantArtStatusItem list option
}