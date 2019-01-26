namespace DeviantArtFs.Requests.User

open DeviantArtFs

type FriendsRequest() =
    member val Username: string = null with get, set

module Friends =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: FriendsRequest) = async {
        let query = seq {
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s?%s" (Dafs.urlEncode req.Username)
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<DeviantArtFriendRecord>.Parse
    }

    let ToAsyncSeq token offset req = AsyncExecute token |> Dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun w -> w :> IBclDeviantArtFriendRecord)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun w -> w :> IBclDeviantArtFriendRecord)
        |> Async.StartAsTask