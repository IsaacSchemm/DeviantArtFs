namespace DeviantArtFs.Requests.Collections

open System.IO
open DeviantArtFs

module CreateCollectionFolder =
    let AsyncExecute token (folder: string) = async {
        let query = seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBody <- String.concat "&" query |> Dafs.stringToBytes

        let! json = Dafs.asyncRead req
        return DeviantArtCollectionFolder.Parse json
    }

    let ExecuteAsync token folder =
        AsyncExecute token folder
        |> AsyncThen.map (fun f -> f :> IBclDeviantArtCollectionFolder)
        |> Async.StartAsTask