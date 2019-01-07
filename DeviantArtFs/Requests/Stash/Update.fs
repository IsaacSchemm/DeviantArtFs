namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open System.IO

[<RequireQualifiedAccess>]
type FieldChange =
    | UpdateTo of string
    | ResetToNull
    | NoChange

type UpdateParameters = {
    Stackid: int64
    Title: FieldChange
    Description: FieldChange
}

module Update =
    let AsyncExecute token (req: UpdateParameters) = async {
        let query = seq {
            match req.Title with
            | FieldChange.UpdateTo s -> yield sprintf "title=%s" (dafs.urlEncode s)
            | FieldChange.ResetToNull -> failwithf "The title cannot be null"
            | FieldChange.NoChange -> ()
            match req.Description with
            | FieldChange.UpdateTo "null" -> failwithf "The string \"null\" is not allowed"
            | FieldChange.UpdateTo s -> yield sprintf "description=%s" (dafs.urlEncode s)
            | FieldChange.ResetToNull -> yield "description=null"
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

    let UpdateTitleAsync token stackid title =
        {
            Stackid = stackid
            Title = 
                match title with
                | null -> FieldChange.ResetToNull
                | _ -> FieldChange.UpdateTo title
            Description = FieldChange.NoChange
        }
        |> AsyncExecute token
        |> Async.StartAsTask
        :> System.Threading.Tasks.Task

    let UpdateDescriptionAsync token stackid description =
        {
            Stackid = stackid
            Title = FieldChange.NoChange
            Description =
                match description with
                | null -> FieldChange.ResetToNull
                | _ -> FieldChange.UpdateTo description
        }
        |> AsyncExecute token
        |> Async.StartAsTask
        :> System.Threading.Tasks.Task