namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open System

module DeviationById =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" id
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> Deviation.Parse
    }

    let ExecuteAsync token id = AsyncExecute token id |> AsyncThen.map (fun o -> o :> IBclDeviation) |> Async.StartAsTask
