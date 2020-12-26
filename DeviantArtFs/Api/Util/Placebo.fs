namespace DeviantArtFs.Api.Util

open DeviantArtFs

module Placebo =
    let AsyncIsValid token = async {
        try
            let req = Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/placebo" Seq.empty
            let! json = Dafs.asyncRead req
            ignore json
            return true
        with
            | :? DeviantArtException as e when e.ResponseBody.error = Some "invalid_token" -> return false
    }

    let IsValidAsync token = AsyncIsValid token |> Async.StartAsTask