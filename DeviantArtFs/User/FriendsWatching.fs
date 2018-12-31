namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data
open System.Net

type internal FriendsWatchingResponse = JsonProvider<"""{
    "watching": true
}""">

module FriendsWatching =
    let AsyncExecute token username = async {
        let req =
            username
            |> WebUtility.UrlEncode
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watching/%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = FriendsWatchingResponse.Parse json
        return o.Watching
    }

    let ExecuteAsync token username = AsyncExecute token username |> Async.StartAsTask