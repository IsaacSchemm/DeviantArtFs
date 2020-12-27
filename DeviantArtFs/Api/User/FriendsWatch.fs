namespace DeviantArtFs.Api.User

open DeviantArtFs

type FriendsWatchRequest(username: string) =
    member __.Username = username
    member val Friend = true with get, set
    member val Deviations = true with get, set
    member val Journals = true with get, set
    member val ForumThreads = true with get, set
    member val Critiques = true with get, set
    member val Scraps = false with get, set
    member val Activity = true with get, set
    member val Collections = true with get, set

module FriendsWatch =
    let AsyncExecute token (ps: FriendsWatchRequest) =
        seq {
            yield sprintf "watch[friend]=%b" ps.Friend
            yield sprintf "watch[deviations]=%b" ps.Deviations
            yield sprintf "watch[journals]=%b" ps.Journals
            yield sprintf "watch[forum_threads]=%b" ps.ForumThreads
            yield sprintf "watch[critiques]=%b" ps.Critiques
            yield sprintf "watch[scraps]=%b" ps.Scraps
            yield sprintf "watch[activity]=%b" ps.Activity
            yield sprintf "watch[collections]=%b" ps.Collections
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watch/%s" (Dafs.urlEncode ps.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token ps =
        AsyncExecute token ps
        |> Async.StartAsTask