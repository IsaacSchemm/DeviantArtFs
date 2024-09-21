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

    let PageCommentsAsync token maxdepth subject replyType limit offset =
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
        }
        |> Utils.get token url
        |> Utils.readAsync
        |> Utils.thenParse<CommentPage>

    let GetCommentsAsync token maxdepth subject scope batchsize offset = Utils.buildTaskSeq {
        initial_offset = offset
        get_page = (fun offset -> PageCommentsAsync token maxdepth subject scope batchsize offset)
        extract_data = (fun page -> page.thread)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
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

    let PageCommentSiblingsAsync token commentid ext_item limit offset =
        seq {
            match ext_item with
            | IncludeRelatedItem true -> "ext_item", "1"
            | IncludeRelatedItem false -> "ext_item", "0"
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/comments/{Utils.guidString commentid}/siblings"
        |> Utils.readAsync
        |> Utils.thenMap (fun str -> str.Replace(""""context": list""", """"context":{}"""))
        |> Utils.thenParse<CommentSiblingsPage>

    let GetCommentSiblingsAsync token commentid ext_item batchsize offset = Utils.buildTaskSeq {
        initial_offset = offset
        get_page = (fun offset -> PageCommentSiblingsAsync token commentid ext_item batchsize offset)
        extract_data = (fun page -> page.thread)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let PostCommentAsync token subject replyType body =
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
        }
        |> Utils.post token url
        |> Utils.readAsync
        |> Utils.thenParse<Comment>