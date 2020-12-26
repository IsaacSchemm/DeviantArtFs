namespace DeviantArtFs.Api.User

open DeviantArtFs

module Whois =
    let AsyncExecute token common usernames = async {
        let query = seq {
            for u in usernames do
                yield u |> Dafs.urlEncode |> sprintf "usernames[]=%s"
                yield! QueryFor.commonParams common
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whois"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtUser>>
    }

    let ExecuteAsync token common usernames =
        AsyncExecute token common usernames
        |> Async.StartAsTask