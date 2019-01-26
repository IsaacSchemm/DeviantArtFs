namespace DeviantArtFs.Requests.User

open DeviantArtFs

module Whoami =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = Dafs.asyncRead req
        return Dafs.parseUser json
    }

    let ExecuteAsync token = AsyncExecute token |> AsyncThen.map Dafs.asBclUser |> Async.StartAsTask