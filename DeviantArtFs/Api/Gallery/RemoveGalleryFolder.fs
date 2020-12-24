namespace DeviantArtFs.Api.Gallery

open System
open DeviantArtFs

module RemoveGalleryFolder =
    let AsyncExecute token (folderid: Guid) =
        Seq.empty
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token folderid =
        AsyncExecute token folderid
        |> Async.StartAsTask