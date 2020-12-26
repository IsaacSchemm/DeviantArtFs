﻿namespace DeviantArtFs.Api.User

open DeviantArtFs

module Whois =
    let AsyncExecute token expansion usernames =
        seq {
            for u in usernames do
                yield u |> Dafs.urlEncode |> sprintf "usernames[]=%s"
                yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/whois"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtUser>>

    let ExecuteAsync token expansion usernames =
        AsyncExecute token expansion usernames
        |> Async.StartAsTask