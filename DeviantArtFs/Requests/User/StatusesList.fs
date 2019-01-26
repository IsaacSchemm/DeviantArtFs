namespace DeviantArtFs.Requests.User

open DeviantArtFs

module StatusesList =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (username: string) = async {
        let query = seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtStatus>.Parse json
    }

    let ToAsyncSeq token offset req = AsyncExecute token |> Dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtStatus)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging username = AsyncExecute token paging username |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtStatus) |> Async.StartAsTask