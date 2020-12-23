namespace DeviantArtFs.Api.User

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
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        let o = Json.deserialize<FriendsWatchingResponse> json
        return o.watching
    }

    let ExecuteAsync token username = AsyncExecute token username |> Async.StartAsTask