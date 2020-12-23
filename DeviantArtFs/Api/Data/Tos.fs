namespace DeviantArtFs.Api.Data

open DeviantArtFs

module Tos =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/data/tos"
        let! json = Dafs.asyncRead req
        return DeviantArtTextOnlyResponse.ParseString json
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask