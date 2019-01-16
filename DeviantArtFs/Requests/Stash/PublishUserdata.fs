namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

module PublishUserdata =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"

        let! json = dafs.asyncRead req
        return StashPublishUserdataResult.Parse json
    }

    let ExecuteAsync token = AsyncExecute token |> iop.thenTo (fun r -> r :> IBclStashPublishUserdataResult) |> Async.StartAsTask