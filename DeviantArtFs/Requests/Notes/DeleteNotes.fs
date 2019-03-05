namespace DeviantArtFs.Requests.Notes

open System
open DeviantArtFs

module DeleteNotes =
    open System.IO

    let AsyncExecute token (noteids: seq<Guid>) = async {
        let query = seq {
            let mutable i = 0
            for g in noteids do
                yield sprintf "noteids[%d]=%O" i g
                i <- i + 1
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/notes/delete"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return ignore json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask :> System.Threading.Tasks.Task
