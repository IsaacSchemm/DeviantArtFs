namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Position =
    let AsyncExecute token (stackid: int64) (position: int) =
        seq {
            yield sprintf "position=%d" position
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/position/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token stackid position =
        AsyncExecute token stackid position
        |> Async.StartAsTask