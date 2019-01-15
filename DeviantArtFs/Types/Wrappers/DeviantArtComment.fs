namespace DeviantArtFs

open System

type DeviantArtCommentHideType =
| None = 0
| HiddenByOwner = 1
| HiddenByAdmin = 2
| HiddenByCommenter = 3
| HiddenAsSpam = 4
| UnknownReason = 5

type IBclDeviantArtComment =
    abstract member Commentid: Guid
    abstract member Parentid: Nullable<Guid>
    abstract member Posted: DateTimeOffset
    abstract member Replies: int
    abstract member Hidden: DeviantArtCommentHideType
    abstract member Body: string
    abstract member User: IBclDeviantArtUser

type DeviantArtComment(original: CommentResponse.Root) =
    member __.Commentid = original.Commentid
    member __.Parentid = original.Parentid
    member __.Posted = original.Posted
    member __.Replies = original.Replies
    member __.Hidden =
        match original.Hidden with
        | None -> DeviantArtCommentHideType.None
        | Some "hidden_by_owner" -> DeviantArtCommentHideType.HiddenByOwner
        | Some "hidden_by_admin" -> DeviantArtCommentHideType.HiddenByAdmin
        | Some "hidden_by_commenter" -> DeviantArtCommentHideType.HiddenByCommenter
        | Some "hidden_as_spam" -> DeviantArtCommentHideType.HiddenAsSpam
        | Some _ -> DeviantArtCommentHideType.UnknownReason
    member __.Body = original.Body
    member __.User = {
        new IBclDeviantArtUser with
            member __.Userid = original.User.Userid
            member __.Username = original.User.Username
            member __.Usericon = original.User.Usericon
            member __.Type = original.User.Type
    }

    interface IBclDeviantArtComment with
        member this.Body = this.Body
        member this.Commentid = this.Commentid
        member this.Hidden = this.Hidden
        member this.Parentid = this.Parentid |> Option.toNullable
        member this.Posted = this.Posted
        member this.Replies = this.Replies
        member this.User = this.User