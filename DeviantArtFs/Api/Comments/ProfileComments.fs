namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type ProfileCommentsRequest(username: string) =
    member __.Username = username
    member val Commentid = Nullable<Guid>() with get, set
    member val Maxdepth = 0 with get, set

module ProfileComments =
    let AsyncExecute token common paging (req: ProfileCommentsRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "maxdepth=%d" req.Maxdepth
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/profile/%s" req.Username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCommentPagedResult>

    let AsyncGetPage token common req limit offset =
        AsyncExecute token common { Offset = offset; Limit = limit } req

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (AsyncGetPage token common req DeviantArtPagingParams.Max)

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask