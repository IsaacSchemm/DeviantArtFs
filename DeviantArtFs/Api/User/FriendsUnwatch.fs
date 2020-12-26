namespace DeviantArtFs.Api.User

open DeviantArtFs

module FriendsUnwatch =
    let AsyncExecute token common username =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/%s" (Dafs.urlEncode username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token common username =
        AsyncExecute token common username
        |> Async.StartAsTask