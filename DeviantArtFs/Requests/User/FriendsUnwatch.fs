namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System.Net

module FriendsUnwatch =
    let AsyncExecute token username = async {
        let req =
            username
            |> WebUtility.UrlEncode
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        SuccessOrErrorResponse.Parse json |> dafs.assertSuccess
    }

    let ExecuteAsync token username = AsyncExecute token username |> Async.StartAsTask