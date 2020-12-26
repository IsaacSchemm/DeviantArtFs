namespace DeviantArtFs.Api.Comments

open DeviantArtFs
open System
open FSharp.Control

type PostDeviationCommentRequest(deviationid: Guid, body: string) =
    member __.Deviationid = deviationid
    member __.Body = body
    member val Commentid = Nullable<Guid>() with get, set

module PostDeviationComment =
    let AsyncExecute token common (req: PostDeviationCommentRequest) = async {
        let query = seq {
            match Option.ofNullable req.Commentid with
            | Some s -> yield sprintf "commentid=%O" s
            | None -> ()
            yield sprintf "body=%s" (Dafs.urlEncode req.Body)
            yield! QueryFor.commonParams common
        }

        let req = Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/comments/post/deviation/%O" req.Deviationid) Seq.empty
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