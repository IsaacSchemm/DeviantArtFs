namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Topic =
    let AsyncExecute token expansion (topic: string) paging =
        seq {
            match Option.ofObj topic with
            | Some s -> yield sprintf "topic=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let ToAsyncSeq token expansion topic offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion topic)

    let ToArrayAsync token expansion topic offset limit =
        ToAsyncSeq token expansion topic offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion topic paging =
        AsyncExecute token expansion topic paging
        |> Async.StartAsTask