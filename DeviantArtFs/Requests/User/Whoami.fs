namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Data

module Whoami =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = dafs.asyncRead req
        return dafs.parseUser json
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask