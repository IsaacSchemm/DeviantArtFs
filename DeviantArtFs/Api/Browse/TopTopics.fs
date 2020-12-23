namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module TopTopics =
    let AsyncExecute token = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse<DeviantArtTopic>.ParseList json
    }

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask