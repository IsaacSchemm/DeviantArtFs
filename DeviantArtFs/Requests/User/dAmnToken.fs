namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Json

type internal dAmnTokenResponse = {
    damntoken: string
}

module dAmnToken =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        let! json = dafs.asyncRead req
        let o = Json.deserialize<dAmnTokenResponse> json
        return o.damntoken
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask