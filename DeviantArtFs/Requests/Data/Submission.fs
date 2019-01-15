namespace DeviantArtFs.Requests.Data

open DeviantArtFs

module Submission =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        let! json = dafs.asyncRead req
        let o = TextOnlyResponse.Parse json
        return o.text
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask