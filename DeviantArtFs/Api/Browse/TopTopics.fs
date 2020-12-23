namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module TopTopics =
    let AsyncExecute token common = async {
        let query = seq {
            yield! QueryFor.commonParams common
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/toptopics?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse<DeviantArtTopic>.ParseList json
    }

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask