﻿namespace DeviantArtFs.Api.Data

open DeviantArtFs

module Privacy =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask