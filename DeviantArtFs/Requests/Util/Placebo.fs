namespace DeviantArtFs.Requests.Util

open DeviantArtFs

module Placebo =
    let AsyncIsValid (token: IDeviantArtAccessToken) = async {
        try
            let no_auto_refresh = {
                new IDeviantArtAccessToken with
                    member __.AccessToken = token.AccessToken
            }
            let req = Dafs.createRequest no_auto_refresh "https://www.deviantart.com/api/v1/oauth2/placebo"
            let! json = Dafs.asyncRead req
            ignore json
            return true
        with
            | :? DeviantArtException as e when e.ResponseBody.error = Some "invalid_token" -> return false
    }

    let IsValidAsync token = AsyncIsValid token |> Async.StartAsTask