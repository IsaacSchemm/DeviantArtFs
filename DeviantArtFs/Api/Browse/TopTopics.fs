namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module TopTopics =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtTopic>>
        |> Dafs.extractResults

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask