namespace DeviantArtFs.Api.Collections

open System
open DeviantArtFs

type RemoveCollectionFolderResponse = {
    success: bool
}

module RemoveCollectionFolder =
    let AsyncExecute token (folderid: Guid) =
        Seq.empty
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<RemoveCollectionFolderResponse>

    let ExecuteAsync token folderid =
        AsyncExecute token folderid
        |> Async.StartAsTask