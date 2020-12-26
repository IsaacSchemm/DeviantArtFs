namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Stack =
    let AsyncExecute token common (stackid: int64) =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let ExecuteAsync token common stackid =
        AsyncExecute token common stackid
        |> Async.StartAsTask