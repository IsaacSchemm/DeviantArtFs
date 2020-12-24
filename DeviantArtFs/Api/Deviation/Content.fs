namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

module Content =
    let AsyncExecute token common (deviationid: Guid) =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/content?deviationid=%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationTextContent>

    let ExecuteAsync token common deviationid =
        AsyncExecute token common deviationid
        |> Async.StartAsTask