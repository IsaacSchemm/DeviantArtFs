namespace DeviantArtFs.Requests.Notes

open System
open DeviantArtFs

type CreateNotesFolderRequest(title: string) =
    member __.Title = title
    member val Parentid = Nullable<Guid>() with get, set

module CreateNotesFolder =
    open System.IO

    let AsyncExecute token (req: CreateNotesFolderRequest) = async {
        let query = seq {
            yield sprintf "title=%s" (Dafs.urlEncode req.Title)
            if req.Parentid.HasValue then
                yield sprintf "parentid=%O" req.Parentid.Value
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/notes/folders/create"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return DeviantArtNewNotesFolder.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask