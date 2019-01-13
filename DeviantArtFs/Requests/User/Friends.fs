namespace DeviantArtFs.Requests.User

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal FriendsElement = JsonProvider<"""{
    "user": {},
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
    User: IDeviantArtUser
    IsWatching: bool
    WatchesYou: bool
    Watch: IWatchInfo
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
                User = r.User.JsonValue.ToString() |> dafs.parseUser
                IsWatching = r.IsWatching
                WatchesYou = r.WatchesYou
                Watch = {
                    new IWatchInfo with
                        member __.Friend = r.Watch.Friend
                        member __.Deviations = r.Watch.Deviations
                        member __.Journals = r.Watch.Journals
                        member __.ForumThreads = r.Watch.ForumThreads
                        member __.Critiques = r.Watch.Critiques
                        member __.Scraps = r.Watch.Scraps
                        member __.Activity = r.Watch.Activity
                        member __.Collections = r.Watch.Collections
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