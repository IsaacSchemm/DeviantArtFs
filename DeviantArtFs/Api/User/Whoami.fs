namespace DeviantArtFs.Api.User

open DeviantArtFs

module Whoami =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtUser>

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask