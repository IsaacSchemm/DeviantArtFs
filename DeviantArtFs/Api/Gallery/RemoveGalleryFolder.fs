namespace DeviantArtFs.Api.Gallery

open System
open DeviantArtFs

module RemoveGalleryFolder =
    let AsyncExecute token common (folderid: Guid) =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token common folderid =
        AsyncExecute token common folderid
        |> Async.StartAsTask