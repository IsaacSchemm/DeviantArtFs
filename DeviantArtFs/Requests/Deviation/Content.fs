namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

module Content =
    let AsyncExecute token (deviationid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/content?deviationid=%O" deviationid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return ContentResponse.Parse json |> DeviationTextContent
    }

    let ExecuteAsync token deviationid =
        AsyncExecute token deviationid
        |> iop.thenTo (fun c -> c :> IBclDeviationTextContent)
        |> Async.StartAsTask