namespace DeviantArtFs

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

    member this.GetEmbeddedDeviations() =
        this.items
        |> Option.defaultValue List.empty
        |> Seq.choose (fun i -> i.deviation)

    member this.GetEmbeddedStatuses() =
        this.items
        |> Option.defaultValue List.empty
        |> Seq.choose (fun i -> i.status)

    interface IDeviantArtDeletable with
        member this.IsDeleted = this.is_deleted