﻿namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

type UndiscoveredRequest() =
    member val CategoryPath = null with get, set

module Undiscovered =
    let AsyncExecute token (paging: PagingParams) (req: UndiscoveredRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/undiscovered?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 120 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req|> iop.thenMapResult (fun o -> o :> IBclDeviation) |> Async.StartAsTask
