namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

module Contents =
    open FSharp.Control

    let RootStack = 0L

    let AsyncExecute token (paging: IDeviantArtPagingParams) (stackid: int64) = async {
        let query = seq {
            yield! queryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents?%s" stackid
            |> dafs.createRequest token

        let! json = dafs.asyncRead req
        return DeviantArtPagedResult<StashMetadata>.Parse json
    }

    let ToAsyncSeq token stackid offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 stackid

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun i -> i :> IBclStashMetadata)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging stackid =
        AsyncExecute token paging stackid
        |> AsyncThen.mapPagedResult (fun i -> i :> IBclStashMetadata)
        |> Async.StartAsTask