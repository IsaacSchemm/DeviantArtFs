namespace DeviantArtFs.Api.Notes

open System
open DeviantArtFs

type RenameNotesFolderRequest(folderid: Guid, title: string) =
    member __.Folderid = folderid
    member __.Title = title

type RenameNotesFolderResponse = { title: string }

module RenameNotesFolder =
    open System.IO

    let AsyncExecute token (req: RenameNotesFolderRequest) = async {
        let query = seq {
            yield sprintf "title=%s" (Dafs.urlEncode req.Title)
        }

        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/notes/folders/rename/%O" req.Folderid
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return DeviantArtRenamedNotesFolder.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask