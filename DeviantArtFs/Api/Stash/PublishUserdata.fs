namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module PublishUserdata =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashPublishUserdataResult>

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask