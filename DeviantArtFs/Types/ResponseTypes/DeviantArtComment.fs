namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtComment = {
    commentid: Guid
    parentid: Guid option
    posted: DateTimeOffset
    replies: int
    [<JsonField(EnumValue = EnumMode.Name)>]
    hidden: string option
    body: string
    user: DeviantArtUser
} with
    static member Parse json = Json.deserialize<DeviantArtComment> json
    member this.GetParentId() = OptUtils.guidDefault this.parentid
    member this.GetHideType() = OptUtils.stringDefault this.hidden