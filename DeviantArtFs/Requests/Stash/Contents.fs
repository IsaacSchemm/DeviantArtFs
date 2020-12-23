namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

module Contents =
    open FSharp.Control

    let RootStack = 0L

    let AsyncExecute token (paging: IDeviantArtPagingParams) (stackid: int64) = async {
        let query = seq {
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents?%s" stackid
            |> Dafs.createRequest token DeviantArtCommonParams.Default

        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<StashMetadata>.Parse json
    }

    let ToAsyncSeq token offset stackid =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset stackid

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging stackid =
        AsyncExecute token paging stackid
        |> Async.StartAsTask