namespace DeviantArtFs.Api.User

open DeviantArtFs
open System

module StatusById =
    let AsyncExecute token (id: Guid) =
        Seq.empty
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtStatus>

    let ExecuteAsync token id =
        AsyncExecute token id
        |> Async.StartAsTask