namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

module WhoFaved =
    open System.Runtime.InteropServices
    open FSharp.Control

    let internal epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)

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
        return json |> dafs.parsePage DeviantArtWhoFavedUser.Parse
    }

    let ToAsyncSeq token deviationid offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 deviationid

    let ToArrayAsync token deviationid ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token deviationid offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun w -> w :> IDeviantArtWhoFavedUser)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token deviationid paging = AsyncExecute token deviationid paging |> iop.thenMapResult (fun w -> w :> IDeviantArtWhoFavedUser) |> Async.StartAsTask