namespace DeviantArtFs.Api.Collections

open System
open DeviantArtFs

module RemoveCollectionFolder =
    let AsyncExecute token (folderid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let ExecuteAsync token folderid =
        AsyncExecute token folderid
        |> Async.StartAsTask