namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data
open System.Net
open System.IO

type WhoisRepsonse = JsonProvider<"""{
    "results": [
        {
            "userid": "408CF9B3-758F-42A2-5167-14DACBF2933E",
            "username": "justgalym",
            "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
            "type": "admin"
        }
    ]
}""">

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
        let o = WhoisRepsonse.Parse json
        return seq {
            for u in o.Results do
                yield {
                    Userid = u.Userid
                    Username = u.Username
                    Usericon = u.Usericon
                    Type = u.Type
                }
        }
    }

    let ExecuteAsync token usernames = AsyncExecute token usernames |> Async.StartAsTask