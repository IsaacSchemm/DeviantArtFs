namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

module Delete =
    open System.IO

    let AsyncExecute token (itemid: int64) = async {
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! sprintf "itemid=%d" itemid |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = Dafs.asyncRead req
        return ignore json
    }

    let ExecuteAsync token itemid = AsyncExecute token itemid |> Async.StartAsTask |> Dafs.toPlainTask