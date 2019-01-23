namespace DeviantArtFs.Requests.User

open DeviantArtFs

module Whoami =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = dafs.asyncRead req
        return dafs.parseUser json
    }

    let ExecuteAsync token = AsyncExecute token |> AsyncThen.map dafs.asBclUser |> Async.StartAsTask