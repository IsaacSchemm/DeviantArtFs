namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type DeviationCommentsRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val Commentid = Nullable<Guid>() with get, set
    member val Maxdepth = 0 with get, set

module DeviationComments =
    let AsyncExecute token common paging (req: DeviationCommentsRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/deviation/%O" req.Deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentPagedResult>

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