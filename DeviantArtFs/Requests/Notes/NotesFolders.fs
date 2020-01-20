namespace DeviantArtFs.Requests.Notes

open DeviantArtFs
open FSharp.Control

module NotesFolders =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/notes/folders"

        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse<DeviantArtNotesFolder>.ParseList json
    }

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask