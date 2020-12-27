namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type PostStatusCommentRequest(statusid: Guid, body: string) =
    member __.Statusid = statusid
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostStatusComment =
    let AsyncExecute token (req: PostStatusCommentRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/status/%O" req.Statusid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask