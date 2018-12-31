namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data
open System.Net

type FriendsSearchRepsonse = JsonProvider<"""{
    "results": [
        {
            "userid": "899C73B5-347B-72C1-2F63-289173EEC881",
            "username": "chris",
            "usericon": "https://a.deviantart.net/avatars/c/h/chris.jpg?3",
            "type": "regular"
        },
        {
            "userid": "C1F3723F-FA8C-D162-298C-D3FD2DBBEE20",
            "username": "chrisfuhrmann",
            "usericon": "https://a.deviantart.net/avatars/c/h/chrisfuhrmann.png?1",
            "type": "regular"
        },
        {
            "userid": "31B1F2C6-99B1-243A-0FE3-3DBA9F1C8B2C",
            "username": "chrisleydon",
            "usericon": "https://a.deviantart.net/avatars/c/h/chrisleydon.png?1",
            "type": "regular"
        },
        {
            "userid": "0EEBE7A1-739E-7479-7FEB-6D5418A0424B",
            "username": "ChristophValentine",
            "usericon": "https://a.deviantart.net/avatars/c/h/christophvalentine.jpg?6",
            "type": "regular"
        }
    ]
}""">

type FriendsSearchRequest(query: string) =
    member __.Query = query
    member val Username = null with get, set

module FriendsSearch =
    let AsyncExecute token (req: FriendsSearchRequest) = async {
        let query = seq {
            yield req.Query |> WebUtility.UrlEncode |> sprintf "query=%s"
            if req.Username |> isNull |> not then
                yield req.Username |> WebUtility.UrlEncode |> sprintf "username=%s"
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/search?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = FriendsSearchRepsonse.Parse json
        return seq {
            for u in o.Results do
                yield {
                    Userid = u.Userid
                    Username = u.Username
                    Usericon = u.Usericon
                    Type = u.Type
                }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask