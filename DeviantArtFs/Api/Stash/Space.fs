namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Space =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/space"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashSpaceResult>

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask