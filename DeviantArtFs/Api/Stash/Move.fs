namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Move =
    let AsyncExecute token (stackid: int64) (targetid: int64) =
        seq {
            yield sprintf "targetid=%d" targetid
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/move/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMoveResult>

    let ExecuteAsync token stackid targetid =
        AsyncExecute token stackid targetid
        |> Async.StartAsTask