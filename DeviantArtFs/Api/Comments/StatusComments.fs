namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type StatusCommentsRequest(statusid: Guid) =
    member __.Statusid = statusid
    member val Commentid = Nullable<Guid>() with get, set
    member val Maxdepth = 0 with get, set

module StatusComments =
    let AsyncExecute token  (req: StatusCommentsRequest) paging =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/status/%O" req.Statusid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentPagedResult>

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