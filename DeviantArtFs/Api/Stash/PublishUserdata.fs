namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module PublishUserdata =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashPublishUserdataResult>

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask