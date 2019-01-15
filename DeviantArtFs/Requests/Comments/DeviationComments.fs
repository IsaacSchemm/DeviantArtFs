namespace DeviantArtFs.Requests.Comments

open DeviantArtFs
open System

type DeviationCommentsRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val Commentid = Nullable<Guid>() with get, set
    member val Maxdepth = 0 with get, set

module DeviationComments =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: DeviationCommentsRequest) = async {
        let query = seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/comments/deviation/%O?%s" req.Deviationid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> CommentPagedResponse.Parse |> DeviantArtCommentPagedResult
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun c -> c :> IBclDeviantArtComment)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenTo (fun r -> r :> IBclDeviantArtCommentPagedResult) |> Async.StartAsTask
