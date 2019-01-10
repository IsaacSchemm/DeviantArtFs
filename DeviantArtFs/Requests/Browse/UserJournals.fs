﻿namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop

type UserJournalsRequest(username: string) =
    member __.Username = username
    member val Featured = true with get, set

module UserJournals =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (req: UserJournalsRequest) (paging: PagingParams) = async {
        let query = seq {
            yield sprintf "username=%s" (dafs.urlEncode req.Username)
            yield sprintf "featured=%b" req.Featured
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/user/journals?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage DeviationResponse.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token req |> dafs.toAsyncSeq offset

    let ToListAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toListAsync
        |> iop.thenMap Deviation
        |> Async.StartAsTask

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenMapResult Deviation |> Async.StartAsTask