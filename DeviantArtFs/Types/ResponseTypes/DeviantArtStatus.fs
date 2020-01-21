namespace DeviantArtFs

open System
open FSharp.Json
open System.Runtime.CompilerServices

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
} with
    static member internal Parse json = Json.deserialize<DeviantArtStatus> json

and DeviantArtStatusUnion =
| Deleted
| Existing of DeviantArtExistingStatus

and DeviantArtExistingStatus = {
    statusid: Guid
    body: string
    ts: DateTimeOffset
    url: string
    comments_count: int
    is_share: bool
    author: DeviantArtUser
    items: DeviantArtStatusItem list
}

[<Extension>]
module DeviantArtStatusExtensions =
    [<Extension>]
    let ToUnion (s: DeviantArtStatus) =
        match s.is_deleted with
        | true -> Deleted
        | false -> Existing {
            statusid = s.statusid.Value
            body = s.body.Value
            ts = s.ts.Value
            url = s.url.Value
            comments_count = s.comments_count.Value
            is_share = s.is_share.Value
            author = s.author.Value
            items = s.items.Value
        }

    [<Extension>]
    let WhereNotDeleted (s: DeviantArtStatus seq) = seq {
        for d in s do
            if not (isNull (d :> obj)) then
                match ToUnion d with
                | Deleted -> ()
                | Existing e -> yield e
    }

    [<Extension>]
    let GetEmbeddedDeviations (seq: DeviantArtStatusItem seq) =
        seq |> Seq.choose (fun i -> i.deviation)

    [<Extension>]
    let GetEmbeddedStatuses (seq: DeviantArtStatusItem seq) =
        seq |> Seq.choose (fun i -> i.status)