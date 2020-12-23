namespace DeviantArtFs.Api.Collections

open System
open System.IO
open DeviantArtFs

module RemoveCollectionFolder =
    let AsyncExecute token (folderid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%A" folderid |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        ignore json
    }

    let ExecuteAsync token folderid = AsyncExecute token folderid |> Async.StartAsTask :> System.Threading.Tasks.Task