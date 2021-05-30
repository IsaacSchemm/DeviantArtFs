namespace DeviantArtFs.ResponseTypes

open System
open FSharp.Json

type CommentHideType =
| hidden_by_owner = 1
| hidden_by_admin = 2
| hidden_by_commenter = 3
| hidden_as_spam = 4

type Comment = {
    commentid: Guid
    parentid: Guid option
    posted: DateTimeOffset
    replies: int
    [<JsonField(EnumValue = EnumMode.Name)>]
    hidden: CommentHideType option
    body: string
    user: User
}