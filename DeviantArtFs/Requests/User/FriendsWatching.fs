namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Json
open System.Net

type FriendsWatchingResponse = {
    watching: bool
}

module FriendsWatching =
    let AsyncExecute token username = async {
        let req =
            username
            |> WebUtility.UrlEncode
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watching/%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = Json.deserialize<FriendsWatchingResponse> json
        return o.watching
    }

    let ExecuteAsync token username = AsyncExecute token username |> Async.StartAsTask