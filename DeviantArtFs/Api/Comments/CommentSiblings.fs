namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type CommentSiblingsRequest(commentid: Guid) =
    member __.Commentid = commentid
    member val ExtItem = false with get, set

module CommentSiblings =
    let AsyncExecute token common paging (req: CommentSiblingsRequest) =
        seq {
            yield sprintf "ext_item=%b" req.ExtItem
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%O/siblings" req.Commentid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentSiblingsPagedResult>

    let ToAsyncSeq token common offset req =
        (fun p -> AsyncExecute token common p req)
        |> Dafs.toAsyncSeq2 offset

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask