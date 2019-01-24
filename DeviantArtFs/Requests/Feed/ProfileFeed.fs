namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

module ProfileFeed =
    open FSharp.Control

    let AsyncExecute token cursor = async {
        let query = seq {
            match cursor with
            | Some s -> yield sprintf "cursor=%s" (dafs.urlEncode s)
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/feed/profile?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtCursorResult<DeviantArtFeedItem>.Parse json
    }

    let ToAsyncSeq token cursor = AsyncExecute token |> dafs.cursorToAsyncSeq cursor

    let ToArrayAsync token cursor limit =
        cursor
        |> Option.ofObj
        |> ToAsyncSeq token
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtFeedItem)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token cursor =
        cursor
        |> Option.ofObj
        |> AsyncExecute token
        |> AsyncThen.mapCursorResult (fun o -> o :> IBclDeviantArtFeedItem)
        |> Async.StartAsTask