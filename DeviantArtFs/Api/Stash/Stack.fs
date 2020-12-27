namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Stack =
    let AsyncExecute token (stackid: int64) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let ExecuteAsync token stackid =
        AsyncExecute token stackid
        |> Async.StartAsTask