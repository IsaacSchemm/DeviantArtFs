namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Data
open System.Net

type FriendsRepsonse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 1,
    "results": [
        {
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
        }
    ]
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type FriendsRequest(username: string) =
    member __.Username = username
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Friends =
    let AsyncExecute token (req: FriendsRequest) = async {
        let query = seq {
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s?%s" (WebUtility.UrlEncode req.Username)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = FriendsRepsonse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = o.Results
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask