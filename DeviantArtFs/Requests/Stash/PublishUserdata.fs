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

module PublishUserdata =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"

        let! json = dafs.asyncRead req
        let resp = PublishUserdataResponse.Parse json
        return {
            new IStashPublishUserdataResult with
                member __.Features = resp.Features :> seq<string>
                member __.Agreements = resp.Agreements :> seq<string>
        }
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask