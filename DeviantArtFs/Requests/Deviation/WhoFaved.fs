namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

type internal WhoFavedElement = JsonProvider<"""{
    "user": {},
    "time": 2222222222
}""">

module WhoFaved =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (deviationid: Guid) = async {
        let query = seq {
            yield sprintf "deviationid=%O" deviationid
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage (fun j ->
            let w = WhoFavedElement.Parse j
            {
                new IWhoFavedUser with
                    member __.User = w.User.JsonValue.ToString() |> dafs.parseUser
                    member __.Time = w.Time
            })
    }

    let ToAsyncSeq token deviationid offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 deviationid

    let ToArrayAsync token deviationid ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token deviationid offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token deviationid paging = AsyncExecute token deviationid paging |> iop.thenCastResult |> Async.StartAsTask