﻿namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Tags =
    let AsyncExecute token paging (tag: string) = async {
        let query = seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags?%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviantArtBrowsePagedResult.Parse json
    }

    let ToAsyncSeq token offset tag =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset tag

    let ToArrayAsync token offset limit tag =
        ToAsyncSeq token offset tag
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging tag =
        AsyncExecute token paging tag
        |> Async.StartAsTask