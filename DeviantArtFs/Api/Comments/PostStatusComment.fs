namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type PostStatusCommentRequest(statusid: Guid, body: string) =
    member __.Statusid = statusid
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostStatusComment =
    let AsyncExecute token common (req: PostStatusCommentRequest) = async {
        let query = seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
            yield! QueryFor.commonParams common
        }
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/status/%O" req.Statusid
            |> Dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query
        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>
    }

    let ExecuteAsync token common req =
        AsyncExecute token common req
        |> Async.StartAsTask