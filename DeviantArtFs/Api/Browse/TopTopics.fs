namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module TopTopics =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtTopic>>

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask