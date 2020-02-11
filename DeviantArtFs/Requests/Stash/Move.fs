namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data
open System.IO

module Move =
    let AsyncExecute token (stackid: int64) (targetid: int64) = async {
        let query = seq {
            yield sprintf "targetid=%d" targetid
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/move/%d" stackid |> Dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return StashMoveResult.Parse json
    }

    let ExecuteAsync token stackid targetid = AsyncExecute token stackid targetid |> Async.StartAsTask