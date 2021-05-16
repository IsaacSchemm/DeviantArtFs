namespace DeviantArtFs.Api

open DeviantArtFs

module Util =
    let AsyncPlacebo token =
        Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/placebo" Seq.empty
        |> Dafs.asyncRead
        |> Dafs.thenMap ignore

    let AsyncIsValid token = async {
        try
            do! AsyncPlacebo token
            return true
        with
            | :? DeviantArtException as e when e.ResponseBody.error = Some "invalid_token" -> return false
    }