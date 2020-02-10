﻿namespace DeviantArtFs.Requests.Comments

open DeviantArtFs
open System
open FSharp.Control

type CommentSiblingsRequest(commentid: Guid) =
    member __.Commentid = commentid
    member val ExtItem = false with get, set

module CommentSiblings =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: CommentSiblingsRequest) = async {
        let query = seq {
            yield sprintf "ext_item=%b" req.ExtItem
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%O/siblings?%s" req.Commentid
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return json |> DeviantArtCommentSiblingsPagedResult.Parse
    }

    let ExecuteAsync token paging req = AsyncExecute token paging req |> Async.StartAsTask
