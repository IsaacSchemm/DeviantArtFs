namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System.Net

module FriendsUnwatch =
    let AsyncExecute token username = async {
        let req =
            username
            |> WebUtility.UrlEncode
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        ignore json
    }

    let ExecuteAsync token username = AsyncExecute token username |> Async.StartAsTask :> System.Threading.Tasks.Task