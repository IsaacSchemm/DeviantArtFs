namespace DeviantArtFs.Requests.Comments

open DeviantArtFs
open System
open System.IO
open FSharp.Control

type PostStatusCommentRequest(statusid: Guid, body: string) =
    member __.Statusid = statusid
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostStatusComment =
    let AsyncExecute token (req: PostStatusCommentRequest) = async {
        let query = seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/status/%O" req.Statusid
            |> Dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query
        let! json = Dafs.asyncRead req
        return json |> DeviantArtComment.Parse
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.map (fun r -> r :> IBclDeviantArtComment) |> Async.StartAsTask
