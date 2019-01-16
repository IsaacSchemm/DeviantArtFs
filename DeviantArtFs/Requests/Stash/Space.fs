namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

module Space =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/space"

        let! json = dafs.asyncRead req
        return StashSpaceResult.Parse json
    }

    let ExecuteAsync token = AsyncExecute token |> iop.thenTo (fun r -> r :> IBclStashSpaceResult) |> Async.StartAsTask