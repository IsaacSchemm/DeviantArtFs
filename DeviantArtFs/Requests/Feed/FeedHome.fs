namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

type FeedHomeRequest() =
    member val Cursor = null with get, set

module FeedHome =
    open FSharp.Control

    let AsyncExecute token (req: FeedHomeRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (dafs.urlEncode s)
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/feed/home?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtFeedPagedResult.Parse json
    }

    let ToAsyncSeq token (req: FeedHomeRequest) = asyncSeq {
        let mutable cursor = req.Cursor
        let mutable has_more = true
        while has_more do
            let! resp = new FeedHomeRequest(Cursor = cursor) |> AsyncExecute token
            for r in resp.items do
                yield r
            cursor <- resp.cursor
            has_more <- resp.has_more
    }

    let ToArrayAsync token req limit =
        ToAsyncSeq token req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtFeedItem)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenTo (fun o -> o :> IBclDeviantArtFeedPagedResult) |> Async.StartAsTask
