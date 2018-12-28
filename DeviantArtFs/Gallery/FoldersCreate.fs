namespace DeviantArtFs.Gallery

open System
open System.IO
open DeviantArtFs
open FSharp.Data

type internal FoldersCreateResponse = JsonProvider<"""{
    "folderid": "E431BAFB-7A00-7EA1-EED7-2EF9FA0F04CE",
    "name": "My New Gallery"
}""">

module FoldersCreate =
    let AsyncExecute token (folder: string) = async {
        let query = seq {
            yield sprintf "folder=%s" folder
        }

        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        let resp = FoldersCreateResponse.Parse json

        // Reuse result type from Folders.fs
        return {
            Folderid = resp.Folderid
            Parent = Nullable()
            Name = resp.Name
            Size = Nullable()
        }
    }

    let ExecuteAsync token folder = AsyncExecute token folder |> Async.StartAsTask