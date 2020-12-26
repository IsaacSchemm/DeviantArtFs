namespace DeviantArtFs.Api.User

open DeviantArtFs

type FriendsWatchingResponse = {
    watching: bool
}

module FriendsWatching =
    let AsyncExecute token common username =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watching/%s" username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<FriendsWatchingResponse>

    let ExecuteAsync token common username =
        AsyncExecute token common username
        |> Async.StartAsTask