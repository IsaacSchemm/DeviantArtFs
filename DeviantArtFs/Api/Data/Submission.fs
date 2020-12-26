namespace DeviantArtFs.Api.Data

open DeviantArtFs

module Submission =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask