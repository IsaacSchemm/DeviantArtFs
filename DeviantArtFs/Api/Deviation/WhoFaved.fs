namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System
open FSharp.Control

module WhoFaved =
    let AsyncExecute token common (deviationid: Guid) paging =
        seq {
            yield sprintf "deviationid=%O" deviationid
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtWhoFavedUser>>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq3 (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common req)

    let ToArrayAsync token common offset limit deviationid =
        ToAsyncSeq token common offset deviationid
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask