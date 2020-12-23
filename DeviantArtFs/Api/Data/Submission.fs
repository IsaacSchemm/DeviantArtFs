namespace DeviantArtFs.Api.Data

open DeviantArtFs

module Submission =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        let! json = Dafs.asyncRead req
        return DeviantArtTextOnlyResponse.ParseString json
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask