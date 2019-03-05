namespace DeviantArtFs.Requests.Notes

open System
open DeviantArtFs

type MarkAs =
| Read = 1
| Unread = 2
| Starred = 3
| NotStarred = 4
| Spam = 5

type MarkNotesRequest(noteids: seq<Guid>, mark_as: MarkAs) =
    member __.Noteids = noteids
    member __.MarkAs = mark_as

module MarkNotes =
    open System.IO

    let AsyncExecute token (req: MarkNotesRequest) = async {
        let query = seq {
            let mutable i = 0
            for g in req.Noteids do
                yield sprintf "noteids[%d]=%O" i g
                i <- i + 1
            match req.MarkAs with
            | MarkAs.Read -> yield "mark_as=read"
            | MarkAs.Unread -> yield "mark_as=unread"
            | MarkAs.Starred -> yield "mark_as=starred"
            | MarkAs.NotStarred -> yield "mark_as=notstarred"
            | MarkAs.Spam -> yield "mark_as=spam"
            | _ -> invalidArg "req" "Invalid \"mark as\" type"
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/notes/mark"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return ignore json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask :> System.Threading.Tasks.Task
