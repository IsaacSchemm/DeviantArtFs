namespace DeviantArtFs.Requests.Notes

open System
open DeviantArtFs
open FSharp.Control

module GetNote =
    let AsyncExecute token (noteid: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/notes/%O" noteid
            |> Dafs.createRequest token

        let! json = Dafs.asyncRead req
        return DeviantArtNote.Parse json
    }

    let ExecuteAsync token noteid =
        AsyncExecute token noteid
        |> AsyncThen.map (fun o -> o :> IBclDeviantArtNote)
        |> Async.StartAsTask