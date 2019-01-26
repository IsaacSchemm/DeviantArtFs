namespace DeviantArtFs.Requests.Collections

open System
open System.IO
open DeviantArtFs

module RemoveCollectionFolder =
    let AsyncExecute token (folderid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%A" folderid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = DeviantArtSuccessOrErrorResponse.Parse json
        dafs.assertSuccess resp
    }

    let ExecuteAsync token folderid = AsyncExecute token folderid |> Async.StartAsTask |> dafs.toPlainTask