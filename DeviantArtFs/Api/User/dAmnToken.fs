namespace DeviantArtFs.Api.User

open DeviantArtFs

type dAmnTokenResponse = {
    damntoken: string
}

module dAmnToken =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        |> Dafs.asyncRead
        |> Dafs.thenParse<dAmnTokenResponse>

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask