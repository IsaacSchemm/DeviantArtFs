namespace DeviantArtFs.Api.Stash

open DeviantArtFs

[<RequireQualifiedAccess>]
type UpdateField =
| Title of string
| Description of string
| ClearDescription

module Update =
    let AsyncExecute token (stackid: int64) (updates: UpdateField seq) = async {
        let query = seq {
            for update in updates do
                match update with
                | UpdateField.Title v -> yield sprintf "title=%s" (Dafs.urlEncode v)
                | UpdateField.Description v -> yield sprintf "description=%s" (Dafs.urlEncode v)
                | UpdateField.ClearDescription -> yield "description=null"
        }

        let req = Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/update/%d" stackid) Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>
    }

    let ExecuteAsync token stackid updates =
        AsyncExecute token stackid updates
        |> Async.StartAsTask