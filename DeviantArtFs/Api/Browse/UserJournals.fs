﻿namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

type UserJournalsRequest(username: string) =
    member __.Username = username
    member val Featured = true with get, set

module UserJournals =
    let AsyncExecute token common paging (req: UserJournalsRequest) = async {
        let query = seq {
            yield sprintf "username=%s" (Dafs.urlEncode req.Username)
            yield sprintf "featured=%b" req.Featured
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/user/journals?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

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