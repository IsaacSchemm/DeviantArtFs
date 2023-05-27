namespace DeviantArtFs.ResponseTypes

open System

type StatusItem = {
    ``type``: string
    status: Status option
    deviation: Deviation option
}

and Status = {
    statusid: Guid option
    body: string option
    ts: DateTimeOffset option
    url: string option
    comments_count: int option
    is_share: bool option
    is_deleted: bool
    author: User option
    items: StatusItem list option
    text_content: EditorText option
}