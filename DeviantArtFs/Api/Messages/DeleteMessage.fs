namespace DeviantArtFs.Api.Messages

open System
open DeviantArtFs

type DeleteMessageRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Messageid = null with get, set
    member val Stackid = null with get, set

module DeleteMessage =
    let AsyncExecute token (req: DeleteMessageRequest) =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            if req.Messageid <> null then
                yield sprintf "messageid=%s" req.Messageid
            if req.Stackid <> null then
                yield sprintf "stackid=%s" req.Stackid
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/messages/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask