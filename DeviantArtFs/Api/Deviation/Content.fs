namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

module Content =
    let AsyncExecute token (deviationid: Guid) =
        Seq.empty
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/content?deviationid=%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationTextContent>

    let ExecuteAsync token deviationid =
        AsyncExecute token deviationid
        |> Async.StartAsTask