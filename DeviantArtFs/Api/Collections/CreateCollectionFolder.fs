namespace DeviantArtFs.Api.Collections

open DeviantArtFs

module CreateCollectionFolder =
    let AsyncExecute token common (folder: string) = async {
        let query = seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
            yield! QueryFor.commonParams common
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCollectionFolder>
    }

    let ExecuteAsync token common folder =
        AsyncExecute token common folder
        |> Async.StartAsTask