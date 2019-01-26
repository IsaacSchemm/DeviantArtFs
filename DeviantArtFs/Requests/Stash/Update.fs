namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open System.IO

type UpdateRequest(stackid: int64) =
    member __.Stackid = stackid
    member val Title = FieldChange<string>.NoChange with get, set
    member val Description = FieldChange<string>.NoChange with get, set

module Update =
    let AsyncExecute token (req: UpdateRequest) = async {
        let query = seq {
            yield! req.Title |> queryFor.fieldChange "title"
            yield! req.Description |> fch.allowNull |> queryFor.fieldChange "description"
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/update/%d" req.Stackid |> dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        DeviantArtSuccessOrErrorResponse.Parse json |> dafs.assertSuccess
    }

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask
        |> dafs.toPlainTask