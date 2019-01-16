namespace DeviantArtFs.Requests.User

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
    open System.IO

    let AsyncExecute token (ps: StatusPostRequest) = async {
        let query = seq {
            match Option.ofObj ps.Body with
            | Some s -> yield sprintf "body=%s" (dafs.urlEncode s)
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
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/statuses/post"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = dafs.asyncRead req
        let result = Json.deserialize<StatusPostResponse> json
        return result.statusid
    }

    let ExecuteAsync token ps = AsyncExecute token ps |> Async.StartAsTask