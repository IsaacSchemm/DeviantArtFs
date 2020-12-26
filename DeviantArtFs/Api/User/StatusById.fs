namespace DeviantArtFs.Api.User

open DeviantArtFs
open System

module StatusById =
    let AsyncExecute token common (id: Guid) =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtStatus>

    let ExecuteAsync token common id =
        AsyncExecute token common id
        |> Async.StartAsTask