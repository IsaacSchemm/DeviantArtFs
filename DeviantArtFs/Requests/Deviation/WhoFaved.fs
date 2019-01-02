namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

type WhoFavedResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "results": [
        {
            "user": {
                "userid": "73A3BB2A-3DAF-86C1-98F8-E1C4BDACF908",
                "username": "semperfiballa",
                "usericon": "https://a.deviantart.net/avatars/s/e/semperfiballa.gif",
                "type": "regular"
            },
            "time": 2222222222
        },
        {
            "user": {
                "userid": "39F9F978-21FC-50F3-3870-6069CC67DFBE",
                "username": "MikeLippa",
                "usericon": "https://a.deviantart.net/avatars/m/i/mikelippa.gif",
                "type": "regular"
            },
            "time": 1162422632
        }
    ]
},
{
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type WhoFavedUser = {
    User: UserResult
    Time: int64
}

type WhoFavedRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module WhoFaved =
    let AsyncExecute token (req: WhoFavedRequest) = async {
        let query = seq {
            yield sprintf "deviationid=%O" req.Deviationid
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = WhoFavedResponse.Parse json
        return {
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            Results = seq {
                for r in resp.Results do
                    yield {
                        User = {
                            Userid = r.User.Userid
                            Username = r.User.Username
                            Usericon = r.User.Usericon
                            Type = r.User.Type
                        }
                        Time = r.Time
                    }
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask