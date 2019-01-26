namespace DeviantArtFs.Requests.Comments

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
            yield! queryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/%O/siblings?%s" req.Commentid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> DeviantArtCommentSiblingsPagedResult.Parse
    }

    let ExecuteAsync token paging req = AsyncExecute token paging req |> AsyncThen.map (fun o -> o :> IBclDeviantArtCommentSiblingsPagedResult) |> Async.StartAsTask
