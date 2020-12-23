namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

module Delete =
    open System.IO

    let AsyncExecute token (itemid: int64) = async {
        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- sprintf "itemid=%d" itemid

        let! json = Dafs.asyncRead req
        return ignore json
    }

    let ExecuteAsync token itemid = AsyncExecute token itemid |> Async.StartAsTask :> System.Threading.Tasks.Task
