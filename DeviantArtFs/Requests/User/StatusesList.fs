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

    let AsyncGetMax token offset req =
        let paging = Dafs.page offset 50
        AsyncExecute token paging req

    let ToAsyncSeq token offset req =
        AsyncGetMax token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtStatus)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtStatus)
        |> Async.StartAsTask

    let GetMaxAsync token paging req =
        AsyncGetMax token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtStatus)
        |> Async.StartAsTask