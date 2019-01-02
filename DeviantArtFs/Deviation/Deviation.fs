namespace DeviantArtFs.Deviation

open DeviantArtFs
open FSharp.Data
open System

module Deviation =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" id
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviationResponse.Parse json
    }

    let ExecuteAsync token id = AsyncExecute token id |> Async.StartAsTask