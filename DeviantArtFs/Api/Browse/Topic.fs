namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Topic =
    let AsyncExecute token common (topic: string) paging =
        seq {
            match Option.ofObj topic with
            | Some s -> yield sprintf "topic=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let ToAsyncSeq token common topic offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common topic)

    let ToArrayAsync token common topic offset limit =
        ToAsyncSeq token common topic offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common topic paging =
        AsyncExecute token common topic paging
        |> Async.StartAsTask