namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Delete =
    let AsyncExecute token (itemid: int64) =
        seq {
            yield sprintf "itemid=%d" itemid
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token itemid =
        AsyncExecute token itemid
        |> Async.StartAsTask