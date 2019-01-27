namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

module FeedHomeBucket =
    open FSharp.Control
    open System

    let AsyncExecute token (paging: IDeviantArtPagingParams) (bucketid: Guid) = async {
        let query = seq {
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/feed/home/%O?%s" bucketid
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token offset bucketid =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset bucketid

    let ToArrayAsync token offset limit bucketid =
        ToAsyncSeq token offset bucketid
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging bucketid =
        AsyncExecute token paging bucketid
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation)
        |> Async.StartAsTask