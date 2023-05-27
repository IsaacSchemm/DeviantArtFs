namespace DeviantArtFs.Api

open DeviantArtFs

module Util =
    let PlaceboAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/placebo"
        |> Utils.readAsync
        |> Utils.thenMap ignore

    let AsyncIsValid token = task {
        try
            do! PlaceboAsync token
            return true
        with
            | :? DeviantArtException as e when e.ResponseBody.error = Some "invalid_token" -> return false
    }