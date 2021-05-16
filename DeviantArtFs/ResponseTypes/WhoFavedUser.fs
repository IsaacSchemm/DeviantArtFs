namespace DeviantArtFs.ResponseTypes

open System
open FSharp.Json

type WhoFavedUser = {
    user: User
    [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
    time: DateTimeOffset
}