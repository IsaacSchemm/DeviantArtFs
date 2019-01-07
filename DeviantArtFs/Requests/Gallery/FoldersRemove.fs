namespace DeviantArtFs.Requests.Gallery

open System
open System.IO
open DeviantArtFs
open FSharp.Data

module FoldersRemove =
    let AsyncExecute token (folderid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%A" folderid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = SuccessOrErrorResponse.Parse json
        dafs.assertSuccess resp
    }

    let ExecuteAsync token folderid = AsyncExecute token folderid |> Async.StartAsTask |> dafs.toPlainTask