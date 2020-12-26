namespace DeviantArtFs.Api.Data

open DeviantArtFs

module Tos =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>
        |> Dafs.extractText

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask