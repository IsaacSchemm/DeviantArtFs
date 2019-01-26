namespace DeviantArtFs.Requests.Messages

open System
open DeviantArtFs

type DeleteMessageRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Messageid = null with get, set
    member val Stackid = null with get, set

module DeleteMessage =
    open System.IO

    let AsyncExecute token (req: DeleteMessageRequest) = async {
        let query = seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            if req.Messageid <> null then
                yield sprintf "messageid=%s" req.Messageid
            if req.Stackid <> null then
                yield sprintf "stackid=%s" req.Stackid
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/messages/delete"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! query |> String.concat "&" |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = Dafs.asyncRead req
        return ignore json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask |> Dafs.toPlainTask