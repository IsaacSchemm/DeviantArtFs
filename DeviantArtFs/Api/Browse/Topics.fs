namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs
open FSharp.Control

type TopicsRequest() =
    member val NumDeviationsPerTopic = Nullable() with get, set

module Topics =
    let AsyncExecute token common paging (req: TopicsRequest) =
        seq {
            match Option.ofNullable req.NumDeviationsPerTopic with
            | Some s -> yield sprintf "topic=%d" s
            | None -> ()
            yield! QueryFor.paging paging 10
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtTopic>>

    let ToAsyncSeq token common offset req =
        (fun p -> AsyncExecute token common p req)
        |> Dafs.toAsyncSeq2 offset

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask