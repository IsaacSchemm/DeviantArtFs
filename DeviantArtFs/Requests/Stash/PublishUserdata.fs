namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data

type internal PublishUserdataResponse = JsonProvider<"""{
    "features": [
        "critique",
        "film"
    ],
    "agreements": [
        "submission_policy",
        "terms_of_service"
    ]
}""">

type PublishUserdataResult = {
    Features: seq<string>
    Agreements: seq<string>
}

module PublishUserdata =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"

        let! json = dafs.asyncRead req
        let resp = PublishUserdataResponse.Parse json
        return {
            Features = resp.Features
            Agreements = resp.Agreements
        }
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask