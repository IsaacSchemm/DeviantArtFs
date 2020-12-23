﻿namespace DeviantArtFs.Api.Util

open DeviantArtFs

module Placebo =
    let AsyncIsValid (token: IDeviantArtAccessToken) = async {
        try
            let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/placebo"
            let! json = Dafs.asyncRead req
            ignore json
            return true
        with
            | :? DeviantArtException as e when e.ResponseBody.error = Some "invalid_token" -> return false
    }

    let IsValidAsync token = AsyncIsValid token |> Async.StartAsTask