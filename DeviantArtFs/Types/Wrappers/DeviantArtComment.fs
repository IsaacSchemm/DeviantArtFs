namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtCommentHideType =
| hidden_by_owner = 1
| hidden_by_admin = 2
| hidden_by_commenter = 3
| hidden_as_spam = 4

type IBclDeviantArtComment =
    abstract member Commentid: Guid
    abstract member Parentid: Nullable<Guid>
    abstract member Posted: DateTimeOffset
    abstract member Replies: int
    abstract member Hidden: Nullable<DeviantArtCommentHideType>
    abstract member Body: string
    abstract member User: IBclDeviantArtUser

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
    interface IBclDeviantArtComment with
        member this.Body = this.body
        member this.Commentid = this.commentid
        member this.Hidden = this.hidden |> Option.toNullable
        member this.Parentid = this.parentid |> Option.toNullable
        member this.Posted = this.posted
        member this.Replies = this.replies
        member this.User = this.user :> IBclDeviantArtUser