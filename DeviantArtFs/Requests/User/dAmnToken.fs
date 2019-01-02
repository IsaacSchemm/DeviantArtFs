namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Data

type dAmnTokenResponse = JsonProvider<"""[{
    "damntoken": "str"
}]""", SampleIsList=true>

module dAmnToken =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        let! json = dafs.asyncRead req
        let o = dAmnTokenResponse.Parse json
        return o.Damntoken
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask