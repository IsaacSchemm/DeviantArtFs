namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

module FeedHomeBucket =
    open FSharp.Control
    open System

    let AsyncExecute token (paging: PagingParams) (bucketid: Guid) = async {
        let query = seq {
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/feed/home/%O?%s" bucketid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage Deviation.Parse json
    }

    let ToAsyncSeq token bucketid offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 bucketid

    let ToArrayAsync token bucketid offset limit =
        ToAsyncSeq token bucketid offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map dafs.asBclDeviation
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging bucketid = AsyncExecute token paging bucketid |> iop.thenMapResult dafs.asBclDeviation |> Async.StartAsTask
