namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

module Content =
    let AsyncExecute token (deviationid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/content?deviationid=%O" deviationid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviationTextContent.Parse json
    }

    let ExecuteAsync token deviationid =
        AsyncExecute token deviationid
        |> AsyncThen.map (fun c -> c :> IBclDeviationTextContent)
        |> Async.StartAsTask