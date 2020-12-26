namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Move =
    let AsyncExecute token common (stackid: int64) (targetid: int64) = async {
        let query = seq {
            yield sprintf "targetid=%d" targetid
            yield! QueryFor.commonParams common
        }

        let req = Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/move/%d" stackid) Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMoveResult>
    }

    let ExecuteAsync token common stackid targetid =
        AsyncExecute token common stackid targetid
        |> Async.StartAsTask