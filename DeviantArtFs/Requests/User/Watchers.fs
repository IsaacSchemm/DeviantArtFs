namespace DeviantArtFs.Requests.User

open DeviantArtFs

type WatchersRequest() =
    member val Username: string = null with get, set

module Watchers =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: WatchersRequest) = async {
        let query = seq {
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s?%s" (Dafs.urlEncode req.Username)
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<DeviantArtWatcherRecord>.Parse
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask