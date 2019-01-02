namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data
open System.IO

type UpdateParameters = {
    Stackid: int64
    Title: string option
    Description: string option
}

module Update =
    let AsyncExecute token (req: UpdateParameters) = async {
        let query = seq {
            match req.Title with
            | Some s -> yield sprintf "title=%s" (dafs.urlEncode s)
            | None -> ()
            match req.Description with
            | Some "null" -> failwithf "The string \"null\" is not allowed"
            | Some null -> yield "description=null"
            | Some s -> yield sprintf "description=%s" (dafs.urlEncode s)
            | None -> ()
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

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask

    let UpdateTitleAsync token stackid title =
        {
            Stackid = stackid
            Title = Some title
            Description = None
        }
        |> AsyncExecute token
        |> Async.StartAsTask

    let UpdateDescriptionAsync token stackid description =
        {
            Stackid = stackid
            Title = None
            Description = Some description
        }
        |> AsyncExecute token
        |> Async.StartAsTask