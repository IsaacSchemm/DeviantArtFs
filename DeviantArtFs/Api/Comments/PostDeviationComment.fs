namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type PostDeviationCommentRequest(deviationid: Guid, body: string) =
    member __.Deviationid = deviationid
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostDeviationComment =
    let AsyncExecute token (req: PostDeviationCommentRequest) =
        seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/deviation/%O" req.Deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtComment>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask