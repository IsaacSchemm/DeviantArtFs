namespace DeviantArtFs.Api.Gallery

open DeviantArtFs

module CreateGalleryFolder =
    let AsyncExecute token (folder: string) =
        seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCollectionFolder>

    let ExecuteAsync token folder =
        AsyncExecute token folder
        |> Async.StartAsTask