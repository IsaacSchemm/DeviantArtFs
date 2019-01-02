namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs
open FSharp.Data

type internal MoreLikeThisResponse = JsonProvider<"""{
    "seed": "C0801604-7894-532E-BC8F-C4EE47273E6D",
    "author": {
        "userid": "899C73B5-347B-72C1-2F63-289173EEC881",
        "username": "chris",
        "usericon": "https://a.deviantart.net/avatars/c/h/chris.jpg?3",
        "type": "regular"
    },
    "more_from_artist": [],
    "more_from_da": []
}""">

type MoreLikeThisResult = {
    Seed: Guid
    Author: UserResult
    MoreFromArtist: seq<DeviationResponse.Root>
    MoreFromDa: seq<DeviationResponse.Root>
}

module MoreLikeThisPreview =
    let AsyncExecute token (seed: Guid) = async {
        let query = seq {
            yield sprintf "seed=%O" seed
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = MoreLikeThisResponse.Parse json
        return {
            Seed = o.Seed
            Author = {
                Userid = o.Author.Userid
                Username = o.Author.Username
                Usericon = o.Author.Usericon
                Type = o.Author.Type
            }
            MoreFromArtist = seq {
                for element in o.MoreFromArtist do
                    let json = element.JsonValue.ToString()
                    yield DeviationResponse.Parse json
            }
            MoreFromDa = seq {
                for element in o.MoreFromDa do
                    let json = element.JsonValue.ToString()
                    yield DeviationResponse.Parse json
            }
        }
    }

    let ExecuteAsync token seed = AsyncExecute token seed |> Async.StartAsTask