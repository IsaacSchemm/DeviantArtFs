namespace DeviantArtFs

open System
open FSharp.Json

type IDeviantArtWhoFavedUser =
    abstract member User: IBclDeviantArtUser
    abstract member Time: DateTimeOffset

type DeviantArtWhoFavedUser = {
    user: IBclDeviantArtUser
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    time: DateTimeOffset
} with
    static member Parse json = Json.deserialize<DeviantArtWhoFavedUser> json
    interface IDeviantArtWhoFavedUser with
        member this.User = this.user
        member this.Time = this.time