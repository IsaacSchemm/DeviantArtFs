namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open System.IO

type UpdateRequest(stackid: int64) =
    member __.Stackid = stackid
    member val Title = DeviantArtFieldChange<string>.NoChange with get, set
    member val Description = DeviantArtFieldChange<string>.NoChange with get, set

module Update =
    let AsyncExecute token (req: UpdateRequest) = async {
        let query = seq {
            yield! req.Title |> QueryFor.fieldChange "title"
            yield! req.Description |> fch.allowNull |> QueryFor.fieldChange "description"
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/update/%d" req.Stackid |> Dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        DeviantArtSuccessOrErrorResponse.Parse json |> Dafs.assertSuccess
    }

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask
        :> System.Threading.Tasks.Task