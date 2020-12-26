namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type CommentSiblingsRequest(commentid: Guid) =
    member __.Commentid = commentid
    member val ExtItem = false with get, set

module CommentSiblings =
    let AsyncExecute token (req: CommentSiblingsRequest) paging =
        seq {
            yield sprintf "ext_item=%b" req.ExtItem
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%O/siblings" req.Commentid)
        |> Dafs.asyncRead
        |> AsyncThen.map (fun str -> str.Replace(""""context": list""", """"context":{}"""))
        |> Dafs.thenParse<DeviantArtCommentSiblingsPagedResult>

    let ToAsyncSeq token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token req)

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> Async.StartAsTask