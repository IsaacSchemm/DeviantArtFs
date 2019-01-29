namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtStatus =
    abstract member Statusid: Guid
    abstract member Body: string
    abstract member Ts: DateTimeOffset
    abstract member Url: string
    abstract member CommentsCount: int
    abstract member IsShare: bool
    abstract member IsDeleted: bool
    abstract member Author: IBclDeviantArtUser
    abstract member EmbeddedDeviations: seq<IBclDeviation>
    abstract member EmbeddedStatuses: seq<IBclDeviantArtStatus>

type PossiblyDeletedDeviantArtStatus = {
    is_deleted: bool
}

type DeviantArtStatusItem = {
    ``type``: string
    status: DeviantArtStatus option
    deviation: Deviation option
}

and DeviantArtStatus = {
    statusid: Guid
    body: string
    ts: DateTimeOffset
    url: string
    comments_count: int
    is_share: bool
    is_deleted: bool
    author: DeviantArtUser
    items: DeviantArtStatusItem list
} with
    static member internal Parse json = Json.deserialize<DeviantArtStatus> json

    static member internal ParseOrNone json =
        let o = Json.deserialize<PossiblyDeletedDeviantArtStatus> json
        if o.is_deleted then
            None
        else
            Some (Json.deserialize<DeviantArtStatus> json)

    member this.EmbeddedDeviations = seq {
        for i in this.items do
            match i.deviation with
                | Some s -> yield s
                | None -> ()
    }

    member this.EmbeddedStatuses = seq {
        for i in this.items do
            match i.status with
                | Some s -> yield s
                | None -> ()
    }

    interface IBclDeviantArtStatus with
        member this.Body = this.body
        member this.CommentsCount = this.comments_count
        member this.EmbeddedDeviations = this.EmbeddedDeviations |> Seq.map (fun s -> s :> IBclDeviation)
        member this.EmbeddedStatuses = this.EmbeddedStatuses |> Seq.map (fun s -> s :> IBclDeviantArtStatus)
        member this.IsDeleted = this.is_deleted
        member this.IsShare = this.is_share
        member this.Statusid = this.statusid
        member this.Ts = this.ts
        member this.Url = this.url
        member this.Author = this.author :> IBclDeviantArtUser