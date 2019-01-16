namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open System

module Download =
    let AsyncExecute token (deviationid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviationDownload.Parse json
    }

    let ExecuteAsync token deviationid = AsyncExecute token deviationid |> iop.thenTo (fun d -> d :> IBclDeviationDownload) |> Async.StartAsTask