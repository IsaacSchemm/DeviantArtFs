namespace DeviantArtFs.Api.Notes

open System
open DeviantArtFs

type SendNoteRequest(``to``: seq<string>) =
    member __.To = ``to``
    member val Subject = null with get, set
    member val Body = null with get, set
    member val Noteid = Nullable<Guid>() with get, set

module SendNote =
    open System.IO

    let AsyncExecute token (req: SendNoteRequest) = async {
        let query = seq {
            let mutable i = 0
            for u in req.To do
                yield sprintf "to[%d]=%s" i (Dafs.urlEncode u)
                i <- i + 1
            if not (isNull req.Subject) then
                yield sprintf "subject=%s" (Dafs.urlEncode req.Subject)
            if not (isNull req.Body) then
                yield sprintf "body=%s" (Dafs.urlEncode req.Body)
            if req.Noteid.HasValue then
                yield sprintf "noteid=%O" req.Noteid.Value
        }

        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/notes/send"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse<DeviantArtSendNoteResult>.ParseList json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask
