namespace DeviantArtFs.Requests.User

open DeviantArtFs

module StatusesList =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (username: string) = async {
        let query = seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtStatus>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtStatus)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapAndWrapPage (fun o -> o :> IBclDeviantArtStatus)
        |> Async.StartAsTask