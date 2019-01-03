namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Data
open System

type WatchersRepsonse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "results": [
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
            "user": {
                "userid": "EDCB4A55-BAE8-C146-B390-5118088A0CF5",
                "username": "muteor",
                "usericon": "https://a.deviantart.net/avatars/m/u/muteor.png?2",
                "type": "regular"
            },
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
    ]
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type WatcherRecord = {
    User: UserResult
    IsWatching: bool
    Lastvisit: DateTimeOffset option
    Watch: WatchInfo
} with
    member this.GetLastVisit() = this.Lastvisit |> Option.toNullable

type WatchersRequest(username: string) =
    member __.Username = username
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Watchers =
    let AsyncExecute token (req: WatchersRequest) = async {
        let query = seq {
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s?%s" (dafs.urlEncode req.Username)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = WatchersRepsonse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for r in o.Results do
                    yield {
                        User = {
                            Userid = r.User.Userid
                            Username = r.User.Username
                            Usericon = r.User.Usericon
                            Type = r.User.Type
                        }
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
                    }
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask