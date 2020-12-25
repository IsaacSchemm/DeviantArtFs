namespace DeviantArtFs.Api.Messages

open DeviantArtFs
open FSharp.Control

module MentionsStack =
    let AsyncExecute token common paging (stackid: string) =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/messages/mentions/%s" (Dafs.urlEncode stackid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let ToAsyncSeq token common offset stackid =
        Dafs.toAsyncSeq3 offset (fun o -> AsyncExecute token common { Offset = o; Limit = DeviantArtPagingParams.Max } stackid)

    let ToArrayAsync token common offset limit stackid =
        ToAsyncSeq token common offset stackid
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging stackid =
        AsyncExecute token common paging stackid
        |> Async.StartAsTask