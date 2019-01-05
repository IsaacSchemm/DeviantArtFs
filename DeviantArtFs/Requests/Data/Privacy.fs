namespace DeviantArtFs.Requests.Data

open DeviantArtFs

module Privacy =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        let! json = dafs.asyncRead req
        let o = TextOnlyResponse.Parse json
        return o.Text
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask