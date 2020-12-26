namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

module Download =
    let AsyncExecute token (deviationid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationDownload>

    let ExecuteAsync token deviationid =
        AsyncExecute token deviationid
        |> Async.StartAsTask