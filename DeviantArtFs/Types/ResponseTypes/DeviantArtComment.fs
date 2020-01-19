namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtCommentHideType =
| hidden_by_owner = 1
| hidden_by_admin = 2
| hidden_by_commenter = 3
| hidden_as_spam = 4

type DeviantArtComment = {
    commentid: Guid
    parentid: Guid option
    posted: DateTimeOffset
    replies: int
    [<JsonField(EnumValue = EnumMode.Name)>]
    hidden: DeviantArtCommentHideType option
    body: string
    user: DeviantArtUser
} with
    static member Parse json = Json.deserialize<DeviantArtComment> json
    member this.GetParentIdOrNull() = Option.toNullable this.parentid
    member this.GetHideTypeOrNull() = Option.toNullable this.hidden