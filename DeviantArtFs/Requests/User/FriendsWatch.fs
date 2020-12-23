namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System.IO
open System.Net

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
    let AsyncExecute token (ps: FriendsWatchRequest) = async {
        let query = seq {
            yield sprintf "watch[friend]=%b" ps.Friend
            yield sprintf "watch[deviations]=%b" ps.Deviations
            yield sprintf "watch[journals]=%b" ps.Journals
            yield sprintf "watch[forum_threads]=%b" ps.ForumThreads
            yield sprintf "watch[critiques]=%b" ps.Critiques
            yield sprintf "watch[scraps]=%b" ps.Scraps
            yield sprintf "watch[activity]=%b" ps.Activity
            yield sprintf "watch[collections]=%b" ps.Collections
        }
        let req =
            ps.Username
            |> WebUtility.UrlEncode
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watch/%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query
        let! json = Dafs.asyncRead req
        ignore json
    }

    let ExecuteAsync token ps = AsyncExecute token ps |> Async.StartAsTask :> System.Threading.Tasks.Task
