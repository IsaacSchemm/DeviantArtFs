namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

module Download =
    let AsyncExecute token common (deviationid: Guid) =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationDownload>

    let ExecuteAsync token common deviationid =
        AsyncExecute token common deviationid
        |> Async.StartAsTask