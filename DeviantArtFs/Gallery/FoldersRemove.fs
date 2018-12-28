namespace DeviantArtFs.Gallery

open System
open System.IO
open DeviantArtFs
open FSharp.Data

type internal FoldersRemoveResponse = JsonProvider<"""{
    "success": true
}""">

module FoldersRemove =
    let AsyncExecute token (folderid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%A" folderid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = FoldersRemoveResponse.Parse json
        return resp.Success
    }

    let ExecuteAsync token folderid = AsyncExecute token folderid |> Async.StartAsTask