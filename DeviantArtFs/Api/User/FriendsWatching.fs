namespace DeviantArtFs.Api.User

open DeviantArtFs

type FriendsWatchingResponse = {
    watching: bool
}

module FriendsWatching =
    let AsyncExecute token username =
        Seq.empty
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watching/%s" username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<FriendsWatchingResponse>

    let ExecuteAsync token username =
        AsyncExecute token username
        |> Async.StartAsTask