namespace DeviantArtFs.Requests.Gallery

open System
open System.IO
open DeviantArtFs
open FSharp.Data

module CreateGalleryFolder =
    let AsyncExecute token (folder: string) = async {
        let query = seq {
            yield sprintf "folder=%s" (dafs.urlEncode folder)
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
        return GalleryFoldersElement.Parse json |> DeviantArtGalleryFolder
    }

    let ExecuteAsync token folder =
        AsyncExecute token folder
        |> iop.thenTo (fun f -> f :> IBclDeviantArtGalleryFolder)
        |> Async.StartAsTask