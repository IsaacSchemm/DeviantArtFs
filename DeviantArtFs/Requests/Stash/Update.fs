namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

type UpdateField =
| Title of string
| Description of string
| ClearDescription

module Update =
    let AsyncExecute token (stackid: int64) (updates: UpdateField seq) = async {
        let query = seq {
            for update in updates do
                match update with
                | Title v -> yield sprintf "title=%s" (Dafs.urlEncode v)
                | Description v -> yield sprintf "description=%s" (Dafs.urlEncode v)
                | ClearDescription -> yield "description=null"
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/update/%d" stackid |> Dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        ignore json
    }

    let ExecuteAsync token stackid updates =
        AsyncExecute token stackid updates
        |> Async.StartAsTask
        :> System.Threading.Tasks.Task