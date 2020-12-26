namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type CommentSiblingsRequest(commentid: Guid) =
    member __.Commentid = commentid
    member val ExtItem = false with get, set

module CommentSiblings =
    let AsyncExecute token common (req: CommentSiblingsRequest) paging =
        seq {
            yield sprintf "ext_item=%b" req.ExtItem
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%O/siblings" req.Commentid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentSiblingsPagedResult>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common req)

    let ToArrayAsync token common req offset limit =
        ToAsyncSeq token common req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask