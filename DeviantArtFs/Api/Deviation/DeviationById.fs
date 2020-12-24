namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

module DeviationById =
    let AsyncExecute token common (id: Guid) =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<Deviation>

    let ExecuteAsync token common id =
        AsyncExecute token common id
        |> Async.StartAsTask
