namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data
open System.IO

module Position =
    let AsyncExecute token (stackid: int64) (position: int) = async {
        let query = seq {
            yield sprintf "position=%d" position
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/position/%d" stackid |> Dafs.createRequest token DeviantArtCommonParams.Default
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        ignore json
    }

    let ExecuteAsync token stackid position = AsyncExecute token stackid position |> Async.StartAsTask :> System.Threading.Tasks.Task
