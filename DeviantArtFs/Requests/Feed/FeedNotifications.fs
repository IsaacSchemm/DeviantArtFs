namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

type FeedNotificationsRequest() =
    member val Cursor = null with get, set

module FeedNotifications =
    open FSharp.Control
    open System

    let AsyncExecute token (req: FeedNotificationsRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (dafs.urlEncode s)
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/feed/notifications?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtFeedPagedResult<DeviantArtFeedNotification>.Parse json
    }

    let ToAsyncSeq token (req: FeedNotificationsRequest) = asyncSeq {
        let mutable cursor = req.Cursor
        let mutable has_more = true
        while has_more do
            let! resp = new FeedNotificationsRequest(Cursor = cursor) |> AsyncExecute token
            for r in resp.items do
                yield r
            cursor <- resp.cursor
            has_more <- resp.has_more
    }

    let ToArrayAsync token req limit =
        ToAsyncSeq token req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtFeedNotification)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req =
        AsyncExecute token req
        |> iop.thenMapFeedResult (fun o -> o :> IBclDeviantArtFeedNotification)
        |> Async.StartAsTask