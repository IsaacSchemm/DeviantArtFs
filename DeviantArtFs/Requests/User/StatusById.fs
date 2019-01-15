namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System

module StatusById =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtStatus.ParseOrNone json
    }

    let ExecuteAsync token id = Async.StartAsTask (async {
        let! status = AsyncExecute token id
        return status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj
    })