namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Delete =
    let AsyncExecute token (itemid: int64) = async {
        let query = seq {
            yield sprintf "itemid=%d" itemid
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/delete" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>
    }

    let ExecuteAsync token itemid =
        AsyncExecute token itemid
        |> Async.StartAsTask