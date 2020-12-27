namespace DeviantArtFs.Api.User

open DeviantArtFs
open FSharp.Control

type WatchersRequest() =
    member val Username: string = null with get, set

module Watchers =
    let AsyncExecute token expansion (req: FriendsRequest) paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtWatcherRecord>>

    let ToAsyncSeq token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion req)

    let ToArrayAsync token expansion req offset limit =
        ToAsyncSeq token expansion req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion req paging =
        AsyncExecute token expansion req paging
        |> Async.StartAsTask