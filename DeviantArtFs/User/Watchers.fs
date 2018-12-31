namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data
open System.Net

type WatchersRepsonse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 1,
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
        }
    ]
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

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
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s?%s" (WebUtility.UrlEncode req.Username)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = WatchersRepsonse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = o.Results
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask