﻿namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

module Tags =
    let AsyncExecute token (paging: IPagingParams) (tag: string) = async {
        let query = seq {
            yield sprintf "tag=%s" (dafs.urlEncode tag)
            yield! queryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation) |> Async.StartAsTask
