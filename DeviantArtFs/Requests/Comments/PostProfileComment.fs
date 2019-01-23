namespace DeviantArtFs.Requests.Comments

open DeviantArtFs
open System
open System.IO
open FSharp.Control

type PostProfileCommentRequest(username: string, body: string) =
    member __.Username = username
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostProfileComment =
    let AsyncExecute token (req: PostProfileCommentRequest) = async {
        let query = seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (dafs.urlEncode req.Body)
        }
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/profile/%s" req.Username
            |> dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = dafs.asyncRead req
        return json |> DeviantArtComment.Parse
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.map (fun r -> r :> IBclDeviantArtComment) |> Async.StartAsTask
