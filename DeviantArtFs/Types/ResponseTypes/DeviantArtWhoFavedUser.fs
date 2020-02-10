namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtWhoFavedUser = {
    user: DeviantArtUser
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    time: DateTimeOffset
} with
    static member Parse json = Json.deserialize<DeviantArtWhoFavedUser> json