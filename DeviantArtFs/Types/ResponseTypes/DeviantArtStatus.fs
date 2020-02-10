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

    member this.GetStatusId() = OptUtils.guidDefault this.statusid
    member this.GetBody() = OptUtils.stringDefault this.body
    member this.GetTs() = OptUtils.timeDefault this.ts
    member this.GetUrl() = OptUtils.stringDefault this.url
    member this.GetCommentsCount() = OptUtils.intDefault this.comments_count
    member this.GetIsShare() = OptUtils.boolDefault this.is_share
    member this.GetAuthor() = OptUtils.recordDefault this.author
    member this.GetItems() = OptUtils.listDefault this.items

    member this.GetEmbeddedDeviations() = seq {
        for i in this.items |> Option.defaultValue List.empty do
            match i.deviation with
                | Some s -> yield s
                | None -> ()
    }

    member this.GetEmbeddedStatuses() = seq {
        for i in this.items |> Option.defaultValue List.empty do
            match i.status with
                | Some s -> yield s
                | None -> ()
    }

    interface IDeviantArtDeletable with
        member this.IsDeleted = this.is_deleted