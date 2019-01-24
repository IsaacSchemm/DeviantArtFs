namespace DeviantArtFs.Requests.Data

open DeviantArtFs

module Tos =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        let! json = dafs.asyncRead req
        let o = DeviantArtTextOnlyResponse.Parse json
        return o.text
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask