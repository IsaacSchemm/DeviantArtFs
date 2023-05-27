namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control

module Comments =
    type Subject = OnDeviation of Guid | OnProfile of string | OnStatus of Guid

    type ReplyType = DirectReply | InReplyToComment of Guid with static member Default = DirectReply

    type Depth = Depth of int with static member Default = Depth 0; static member Max = Depth 5

    type CommentPage = {
        has_more: bool
        next_offset: int option
        has_less: bool
        prev_offset: int option
        total: int option
        thread: Comment list
    }

    let PageCommentsAsync token expansion maxdepth subject replyType limit offset =
        let url =
            match subject with
            | OnDeviation g -> $"https://www.deviantart.com/api/v1/oauth2/comments/deviation/{Utils.guidString g}"
            | OnProfile username -> $"https://www.deviantart.com/api/v1/oauth2/comments/profile/{Uri.EscapeDataString username}"
            | OnStatus g -> $"https://www.deviantart.com/api/v1/oauth2/comments/status/{Utils.guidString g}"

        seq {
            match replyType with
            | DirectReply -> ()
            | InReplyToComment g -> yield "commentid", string g
            match maxdepth with
            | Depth x -> yield "maxdepth", string (min x 5)
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token url
        |> Utils.readAsync
        |> Utils.thenParse<CommentPage>

    let GetCommentsAsync token expansion maxdepth subject scope batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageCommentsAsync token expansion maxdepth subject scope batchsize offset
            yield! data.thread
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    type IncludeRelatedItem = IncludeRelatedItem of bool

    type CommentSiblingsContext = {
        parent: Comment option
        item_profile: User option
        item_deviation: Deviation option
        item_status: Status option
    }

    type CommentSiblingsPage = {
        has_more: bool
        next_offset: int option
        has_less: bool
        prev_offset: int option
        thread: Comment list
        context: CommentSiblingsContext
    }

    let PageCommentSiblingsAsync token expansion commentid ext_item limit offset =
        seq {
            match ext_item with
            | IncludeRelatedItem true -> "ext_item", "1"
            | IncludeRelatedItem false -> "ext_item", "0"
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/comments/{Utils.guidString commentid}/siblings"
        |> Utils.readAsync
        |> Utils.thenMap (fun str -> str.Replace(""""context": list""", """"context":{}"""))
        |> Utils.thenParse<CommentSiblingsPage>

    let GetCommentSiblingsAsync token expansion commentid ext_item batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageCommentSiblingsAsync token expansion commentid ext_item batchsize offset
            yield! data.thread
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    let PostCommentAsync token expansion subject replyType body =
        let url =
            match subject with
            | OnDeviation g -> $"https://www.deviantart.com/api/v1/oauth2/comments/post/deviation/{Utils.guidString g}"
            | OnProfile username -> $"https://www.deviantart.com/api/v1/oauth2/comments/post/profile/{Uri.EscapeDataString username}"
            | OnStatus g -> $"https://www.deviantart.com/api/v1/oauth2/comments/post/status/{Utils.guidString g}"

        seq {
            match replyType with
            | DirectReply -> ()
            | InReplyToComment g -> yield "commentid", string g
            yield "body", body
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.post token url
        |> Utils.readAsync
        |> Utils.thenParse<Comment>