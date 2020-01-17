namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtStatus =
    abstract member Statusid: Guid Nullable
    abstract member Body: string
    abstract member Ts: DateTimeOffset Nullable
    abstract member Url: string
    abstract member CommentsCount: int Nullable
    abstract member IsShare: bool Nullable
    abstract member IsDeleted: bool
    abstract member Author: IBclDeviantArtUser
    abstract member EmbeddedDeviations: IBclDeviation seq
    abstract member EmbeddedStatuses: IBclDeviantArtStatus seq

type PossiblyDeletedDeviantArtStatus = {
    is_deleted: bool
}

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

    member this.EmbeddedDeviations = seq {
        for i in this.items |> Option.defaultValue List.empty do
            match i.deviation with
                | Some s -> yield s
                | None -> ()
    }

    member this.EmbeddedStatuses = seq {
        for i in this.items |> Option.defaultValue List.empty do
            match i.status with
                | Some s -> yield s
                | None -> ()
    }

    interface IBclDeviantArtStatus with
        member this.Body = this.body |> Option.defaultValue null
        member this.CommentsCount = this.comments_count |> Option.toNullable
        member this.EmbeddedDeviations = this.EmbeddedDeviations |> Seq.map (fun s -> s :> IBclDeviation)
        member this.EmbeddedStatuses = this.EmbeddedStatuses |> Seq.map (fun s -> s :> IBclDeviantArtStatus)
        member this.IsDeleted = this.is_deleted
        member this.IsShare = this.is_share |> Option.toNullable
        member this.Statusid = this.statusid |> Option.toNullable
        member this.Ts = this.ts |> Option.toNullable
        member this.Url = this.url |> Option.defaultValue null
        member this.Author =
            match this.author with
            | Some a -> a :> IBclDeviantArtUser
            | None -> null