namespace DeviantArtFs.Requests.Notes

open System
open DeviantArtFs

module RemoveNotesFolder =
    let AsyncExecute token (folderid: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/notes/folders/remove/%O" folderid
            |> Dafs.createRequest token
        req.Method <- "POST"

        let! json = Dafs.asyncRead req
        ignore json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask :> System.Threading.Tasks.Task