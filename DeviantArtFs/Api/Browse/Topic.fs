namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Topic =
    let AsyncExecute token common paging (topic: string) =
        seq {
            match Option.ofObj topic with
            | Some s -> yield sprintf "topic=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetPage token common topic limit offset =
        AsyncExecute token common { Offset = offset; Limit = limit } topic

    let ToAsyncSeq token common offset topic =
        Dafs.toAsyncSeq3 offset (AsyncGetPage token common topic DeviantArtPagingParams.Max)

    let ToArrayAsync token common offset limit topic =
        ToAsyncSeq token common offset topic
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging topic =
        AsyncExecute token common paging topic
        |> Async.StartAsTask