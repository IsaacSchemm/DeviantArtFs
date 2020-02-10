namespace DeviantArtFs.Requests.Notes

open System
open DeviantArtFs
open FSharp.Control

module GetNotes =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (folderid: Guid option) = async {
        let query = seq {
            match folderid with
            | Some g -> yield sprintf "folderid=%O" g
            | None -> ()
            yield! QueryFor.paging paging 50
        }

        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/notes?%s"
            |> Dafs.createRequest token

        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtNote>.Parse json
    }

    let ToAsyncSeq token offset folderid =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset folderid

    let ToArrayAsync token folderid offset limit =
        folderid
        |> Option.ofNullable
        |> ToAsyncSeq token offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging folderid =
        folderid
        |> Option.ofNullable
        |> AsyncExecute token paging
        |> Async.StartAsTask