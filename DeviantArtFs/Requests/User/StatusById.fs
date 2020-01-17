namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System

module StatusById =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtStatus.Parse json
    }

    let ExecuteAsync token id = Async.StartAsTask (async {
        let! status = AsyncExecute token id
        return status :> IBclDeviantArtStatus
    })