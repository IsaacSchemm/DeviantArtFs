namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System
open FSharp.Control

module WhoFaved =
    let AsyncExecute token expansion (deviationid: Guid) paging =
        seq {
            yield sprintf "deviationid=%O" deviationid
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtWhoFavedUser>>

    let ToAsyncSeq token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion req)

    let ToArrayAsync token expansion offset limit deviationid =
        ToAsyncSeq token expansion offset deviationid
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion req paging =
        AsyncExecute token expansion req paging
        |> Async.StartAsTask