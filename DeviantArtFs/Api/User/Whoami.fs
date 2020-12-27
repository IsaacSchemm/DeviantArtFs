namespace DeviantArtFs.Api.User

open DeviantArtFs

module Whoami =
    let AsyncExecute token expansion =
        seq {
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtUser>

    let ExecuteAsync token expansion =
        AsyncExecute token expansion
        |> Async.StartAsTask