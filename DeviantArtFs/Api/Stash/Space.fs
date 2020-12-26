namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Space =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/stash/space"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashSpaceResult>

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask