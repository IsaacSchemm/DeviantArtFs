namespace DeviantArtFs.Requests.User

open DeviantArtFs

module StatusesList =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (username: string) = async {
        let query = seq {
            yield sprintf "username=%s" (dafs.urlEncode username)
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtStatus>.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtStatus)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging username = AsyncExecute token paging username |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtStatus) |> Async.StartAsTask