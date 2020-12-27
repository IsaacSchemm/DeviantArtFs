namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type PostProfileCommentRequest(username: string, body: string) =
    member __.Username = username
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostProfileComment =
    let AsyncExecute token (req: PostProfileCommentRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/profile/%s" req.Username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask