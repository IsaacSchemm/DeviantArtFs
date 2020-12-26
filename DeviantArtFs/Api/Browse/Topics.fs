namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs
open FSharp.Control

type TopicsRequest() =
    member val NumDeviationsPerTopic = Nullable() with get, set

module Topics =
    let AsyncExecute token common (req: TopicsRequest) paging =
        seq {
            match Option.ofNullable req.NumDeviationsPerTopic with
            | Some s -> yield sprintf "topic=%d" s
            | None -> ()
            yield! QueryFor.paging paging 10
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtTopic>>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common req)

    let ToArrayAsync token common req offset limit =
        ToAsyncSeq token common req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask