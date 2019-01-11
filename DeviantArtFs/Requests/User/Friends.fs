namespace DeviantArtFs.Requests.User

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal FriendsElement = JsonProvider<"""{
    "user": {
        "userid": "D34F0633-FEFC-5E3B-8983-0B7CD5F7DC9E",
        "username": "Spyed",
        "usericon": "https://a.deviantart.net/avatars/s/p/spyed.gif",
        "type": "regular"
    },
    "is_watching": true,
    "watches_you": false,
    "watch": {
        "friend": true,
        "deviations": false,
        "journals": true,
        "forum_threads": true,
        "critiques": false,
        "scraps": false,
        "activity": false,
        "collections": false
    }
}""">

type FriendRecord = {
    User: DeviantArtUser
    IsWatching: bool
    WatchesYou: bool
    Watch: WatchInfo
}

type FriendsRequest() =
    member val Username: string = null with get, set

module Friends =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: FriendsRequest) = async {
        let query = seq {
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s?%s" (dafs.urlEncode req.Username)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage (fun j ->
            let r = FriendsElement.Parse j
            {
                User = {
                    Userid = r.User.Userid
                    Username = r.User.Username
                    Usericon = r.User.Usericon
                    Type = r.User.Type
                }
                IsWatching = r.IsWatching
                WatchesYou = r.WatchesYou
                Watch = {
                    Friend = r.Watch.Friend
                    Deviations = r.Watch.Deviations
                    Journals = r.Watch.Journals
                    ForumThreads = r.Watch.ForumThreads
                    Critiques = r.Watch.Critiques
                    Scraps = r.Watch.Scraps
                    Activity = r.Watch.Activity
                    Collections = r.Watch.Collections
                }
            })
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToListAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toListAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> iop.thenCastResult |> Async.StartAsTask