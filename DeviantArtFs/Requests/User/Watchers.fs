namespace DeviantArtFs.Requests.User

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data
open System

type WatchersElement = JsonProvider<"""[
{
    "user": {
        "userid": "EDCB4A55-BAE8-C146-B390-5118088A0CF5",
        "username": "muteor",
        "usericon": "https://a.deviantart.net/avatars/m/u/muteor.png?2",
        "type": "regular"
    },
    "is_watching": true,
    "lastvisit": "2014-10-13T22:34:16-0700",
    "watch": {
        "friend": true,
        "deviations": true,
        "journals": true,
        "forum_threads": true,
        "critiques": true,
        "scraps": false,
        "activity": true,
        "collections": true
    }
},
{
    "user": {},
    "is_watching": true,
    "lastvisit": null,
    "watch": {
        "friend": true,
        "deviations": true,
        "journals": true,
        "forum_threads": true,
        "critiques": true,
        "scraps": false,
        "activity": true,
        "collections": true
    }
}
]""", SampleIsList=true>

type WatcherRecord = {
    User: IDeviantArtUser
    IsWatching: bool
    Lastvisit: DateTimeOffset option
    Watch: WatchInfo
} with
    member this.GetLastVisit() = this.Lastvisit |> Option.toNullable

type WatchersRequest() =
    member val Username: string = null with get, set

module Watchers =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: WatchersRequest) = async {
        let query = seq {
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s?%s" (dafs.urlEncode req.Username)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage (fun j ->
            let r = WatchersElement.Parse j
            {
                User = r.User.JsonValue.ToString() |> dafs.parseUser
                IsWatching = r.IsWatching
                Lastvisit = r.Lastvisit
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