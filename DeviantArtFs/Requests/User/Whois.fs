namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System.IO

module Whois =
    let AsyncExecute token usernames = async {
        let query = seq {
            for u in usernames do
                yield u |> dafs.urlEncode |> sprintf "usernames[]=%s"
        }
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whois"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = dafs.asyncRead req
        return dafs.parseListOnly dafs.parseUser json
    }

    let ExecuteAsync token usernames = AsyncExecute token usernames |> iop.thenMap dafs.asBclUser |> Async.StartAsTask