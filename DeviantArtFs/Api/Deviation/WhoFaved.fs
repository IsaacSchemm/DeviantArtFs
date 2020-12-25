namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System
open FSharp.Control

module WhoFaved =
    let AsyncExecute token common paging (deviationid: Guid) =
        seq {
            yield sprintf "deviationid=%O" deviationid
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtWhoFavedUser>>

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (fun o -> AsyncExecute token common { Offset = o; Limit = DeviantArtPagingParams.Max } req)

    let ToArrayAsync token common offset limit deviationid =
        ToAsyncSeq token common offset deviationid
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask