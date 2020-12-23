namespace DeviantArtFs.Requests.Comments

open DeviantArtFs
open System
open FSharp.Control

type StatusCommentsRequest(statusid: Guid) =
    member __.Statusid = statusid
    member val Commentid = Nullable<Guid>() with get, set
    member val Maxdepth = 0 with get, set

module StatusComments =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: StatusCommentsRequest) = async {
        let query = seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/status/%O?%s" req.Statusid
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return json |> DeviantArtCommentPagedResult.Parse
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask