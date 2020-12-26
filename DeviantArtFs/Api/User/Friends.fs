namespace DeviantArtFs.Api.User

open DeviantArtFs
open FSharp.Control

type FriendsRequest() =
    member val Username: string = null with get, set

module Friends =
    let AsyncExecute token common (req: FriendsRequest) paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtFriendRecord>>

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