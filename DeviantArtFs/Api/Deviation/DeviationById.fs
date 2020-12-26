namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

module DeviationById =
    let AsyncExecute token expansion (id: Guid) =
        seq {
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<Deviation>

    let ExecuteAsync token expansion id =
        AsyncExecute token expansion id
        |> Async.StartAsTask
