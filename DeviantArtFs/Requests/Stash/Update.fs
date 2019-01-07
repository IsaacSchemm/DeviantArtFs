namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open System.IO

[<RequireQualifiedAccess>]
type FieldChange =
    | Update of string
    | NoChange

type UpdateRequest(stackid: int64) =
    member __.Stackid = stackid
    member val Title = FieldChange.NoChange with get, set
    member val Description: FieldChange = FieldChange.NoChange with get, set

module Update =
    let AsyncExecute token (req: UpdateRequest) = async {
        let query = seq {
            match req.Title with
            | FieldChange.Update null -> failwithf "A null title is not allowed"
            | FieldChange.Update s -> yield sprintf "title=%s" (dafs.urlEncode s)
            | FieldChange.NoChange -> ()
            match req.Description with
            | FieldChange.Update "null" -> failwithf "The string \"null\" is not allowed"
            | FieldChange.Update null -> yield "description=null"
            | FieldChange.Update s -> yield sprintf "description=%s" (dafs.urlEncode s)
            | FieldChange.NoChange -> ()
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
        SuccessOrErrorResponse.Parse json |> dafs.assertSuccess
    }

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask
        |> dafs.toPlainTask