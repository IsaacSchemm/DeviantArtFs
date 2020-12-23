namespace DeviantArtFs.Api.User

open DeviantArtFs
open System

module StatusById =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviantArtStatus.Parse json
    }

    let ExecuteAsync token id =
        AsyncExecute token id
        |> Async.StartAsTask