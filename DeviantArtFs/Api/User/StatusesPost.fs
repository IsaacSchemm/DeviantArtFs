namespace DeviantArtFs.Api.User

open DeviantArtFs
open FSharp.Json
open System

type StatusPostResponse = {
    statusid: Guid
}

type StatusPostRequest(body: string) =
    member __.Body = body
    member val Statusid = Nullable<Guid>() with get, set
    member val Parentid = Nullable<Guid>() with get, set
    member val Stashid = Nullable<int64>() with get, set

module StatusPost =
    let AsyncExecute token (ps: StatusPostRequest) = async {
        let query = seq {
            match Option.ofObj ps.Body with
            | Some s -> yield sprintf "body=%s" (Dafs.urlEncode s)
            | None -> ()
            match Option.ofNullable ps.Parentid with
            | Some s -> yield sprintf "parentid=%O" s
            | None -> ()
            match Option.ofNullable ps.Statusid with
            | Some s -> yield sprintf "id=%O" s
            | None -> ()
            match Option.ofNullable ps.Stashid with
            | Some s -> yield sprintf "stashid=%O" s
            | None -> ()
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/statuses/post" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<StatusPostResponse>
    }

    let ExecuteAsync token ps =
        AsyncExecute token ps
        |> Async.StartAsTask