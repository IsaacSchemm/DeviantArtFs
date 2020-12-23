namespace DeviantArtFs.Api.User

open DeviantArtFs

module Whoami =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = Dafs.asyncRead req
        return DeviantArtUser.Parse json
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask