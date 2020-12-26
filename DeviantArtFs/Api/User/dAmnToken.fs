namespace DeviantArtFs.Api.User

open DeviantArtFs

type dAmnTokenResponse = {
    damntoken: string
}

module dAmnToken =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        |> Dafs.asyncRead
        |> Dafs.thenParse<dAmnTokenResponse>

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask