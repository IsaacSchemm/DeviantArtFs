namespace DeviantArtFs.Util

open DeviantArtFs
open System.Threading.Tasks

module Placebo =
    let AsyncIsValid token = async {
        try
            let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/placebo"
            let! json = dafs.asyncRead req
            ignore json
            return true
        with
            | :? DeviantArtException as e when e.ResponseBody.Error = "invalid_token" -> return false
    }

    let IsValidAsync token = AsyncIsValid token |> Async.StartAsTask :> Task