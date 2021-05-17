namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Comments =
    let AsyncPageComments token maxdepth subject replyType limit offset =
        let url =
            match subject with
            | OnDeviation g -> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/deviation/%O" g
            | OnProfile username -> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/profile/%s" (Uri.EscapeDataString username)
            | OnStatus g -> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/status/%O" g

        seq {
            yield! QueryFor.commentReplyType replyType
            yield! QueryFor.commentDepth maxdepth
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token url
        |> Dafs.asyncRead
        |> Dafs.thenParse<CommentPage>

    let AsyncGetComments token maxdepth subject scope batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageComments token maxdepth subject scope batchsize)

    let AsyncPageCommentSiblings token commentid ext_item limit offset =
        seq {
            yield! QueryFor.includeRelatedItem ext_item
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%s/siblings" (Dafs.guid2str commentid))
        |> Dafs.asyncRead
        |> Dafs.thenMap (fun str -> str.Replace(""""context": list""", """"context":{}"""))
        |> Dafs.thenParse<CommentSiblingsPage>

    let AsyncGetCommentSiblings token commentid ext_item batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageCommentSiblings token commentid ext_item batchsize)

    let AsyncPostComment token subject replyType body =
        let url =
            match subject with
            | OnDeviation g -> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/deviation/%O" g
            | OnProfile username -> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/profile/%s" (Uri.EscapeDataString username)
            | OnStatus g -> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/status/%O" g

        seq {
            yield! QueryFor.commentReplyType replyType
            yield sprintf "body=%s" (Uri.EscapeDataString body)
        }
        |> Dafs.createRequest Dafs.Method.POST token url
        |> Dafs.asyncRead
        |> Dafs.thenParse<Comment>