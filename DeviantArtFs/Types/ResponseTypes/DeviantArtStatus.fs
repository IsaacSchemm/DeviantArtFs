﻿namespace DeviantArtFs

open System
open FSharp.Json

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
    member this.ToUnion() =
        match this.is_deleted with
        | true -> Deleted
        | false -> Existing {
            statusid = this.statusid.Value
            body = this.body.Value
            ts = this.ts.Value
            url = this.url.Value
            comments_count = this.comments_count.Value
            is_share = this.is_share.Value
            author = this.author.Value
            items = this.items.Value
        }
    member this.SingleIfExists() =
        match this.ToUnion() with
        | Deleted -> Seq.empty
        | Existing e -> Seq.singleton e

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
} with
    member this.GetEmbeddedDeviations() =
        this.items
        |> Seq.map (fun i -> i.deviation)
        |> Seq.choose id
    member this.GetEmbeddedStatuses() =
        this.items
        |> Seq.map (fun i -> i.status)
        |> Seq.choose id