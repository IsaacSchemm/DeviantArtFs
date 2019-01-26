namespace DeviantArtFs.Requests.User

open DeviantArtFs

type WatchersRequest() =
    member val Username: string = null with get, set

module Watchers =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: WatchersRequest) = async {
        let query = seq {
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s?%s" (Dafs.urlEncode req.Username)
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<DeviantArtWatcherRecord>.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> Dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun w -> w :> IBclDeviantArtWatcherRecord)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun w -> w :> IBclDeviantArtWatcherRecord)
        |> Async.StartAsTask