namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System

module StatusesStatus =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%O" id
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return StatusResponse.Parse json |> Status
    }

    let ExecuteAsync token id = AsyncExecute token id |> Async.StartAsTask