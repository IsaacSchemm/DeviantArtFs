namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtWhoFavedUser =
    abstract member User: IBclDeviantArtUser
    abstract member Time: DateTimeOffset

type DeviantArtWhoFavedUser = {
    user: DeviantArtUser
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    time: DateTimeOffset
} with
    static member Parse json = Json.deserialize<DeviantArtWhoFavedUser> json
    interface IBclDeviantArtWhoFavedUser with
        member this.User = this.user :> IBclDeviantArtUser
        member this.Time = this.time