namespace DeivantArtFs.Api

open System
open DeviantArtFs

module Comments =
    type DeviationCommentsRequest(deviationid: Guid) =
        member __.Deviationid = deviationid
        member val Commentid = Nullable<Guid>() with get, set
        member val Maxdepth = 0 with get, set

    let AsyncPageDeviationComments token (req: DeviationCommentsRequest) paging =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/deviation/%O" req.Deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentPagedResult>

    let AsyncGetDeviationComments token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageDeviationComments token req)

    type StatusCommentsRequest(statusid: Guid) =
        member __.Statusid = statusid
        member val Commentid = Nullable<Guid>() with get, set
        member val Maxdepth = 0 with get, set

    let AsyncPageStatusComments token  (req: StatusCommentsRequest) paging =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/status/%O" req.Statusid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentPagedResult>

    let AsyncGetStatusComments token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageStatusComments token req)

    type ProfileCommentsRequest(username: string) =
        member __.Username = username
        member val Commentid = Nullable<Guid>() with get, set
        member val Maxdepth = 0 with get, set

    let AsyncPageProfileComments token (req: ProfileCommentsRequest) paging =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/profile/%s" req.Username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentPagedResult>
        
    let AsyncGetProfileComments token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageProfileComments token req)

    type CommentSiblingsRequest(commentid: Guid) =
        member __.Commentid = commentid
        member val ExtItem = false with get, set

    let AsyncPageCommentSiblings token (req: CommentSiblingsRequest) paging =
        seq {
            yield sprintf "ext_item=%b" req.ExtItem
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%O/siblings" req.Commentid)
        |> Dafs.asyncRead
        |> AsyncThen.map (fun str -> str.Replace(""""context": list""", """"context":{}"""))
        |> Dafs.thenParse<DeviantArtCommentSiblingsPagedResult>

    let AsyncGetCommentSiblings token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageCommentSiblings token req)

    type PostDeviationCommentRequest(deviationid: Guid, body: string) =
        member __.Deviationid = deviationid
        member __.Body = body
        member val Commentid = Nullable<Guid>() with get, set

    let AsyncPostDeviationComment token (req: PostDeviationCommentRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/deviation/%O" req.Deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>

    type PostStatusCommentRequest(statusid: Guid, body: string) =
        member __.Statusid = statusid
        member __.Body = body
        member val Commentid = Nullable<Guid>() with get, set

    let AsyncPostStatusComment token (req: PostStatusCommentRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/status/%O" req.Statusid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>

    type PostProfileCommentRequest(username: string, body: string) =
        member __.Username = username
        member __.Body = body
        member val Commentid = Nullable<Guid>() with get, set

    let AsyncPostProfileComment token (req: PostProfileCommentRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/profile/%s" req.Username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>