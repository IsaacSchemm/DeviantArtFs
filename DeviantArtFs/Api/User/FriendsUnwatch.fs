namespace DeviantArtFs.Api.User

open DeviantArtFs

module FriendsUnwatch =
    let AsyncExecute token username =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/%s" (Dafs.urlEncode username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token username =
        AsyncExecute token username
        |> Async.StartAsTask