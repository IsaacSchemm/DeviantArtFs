namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

module ProfileFeed =
    open FSharp.Control

    let AsyncExecute token cursor = async {
        let query = seq {
            match cursor with
            | Some s -> yield sprintf "cursor=%s" (Dafs.urlEncode s)
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/feed/profile?%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviantArtFeedCursorResult.Parse json
    }

    let ToAsyncSeq token cursor =
        let f c () = AsyncExecute token c
        Dafs.toAsyncSeq cursor () f

    let ToArrayAsync token cursor limit =
        cursor
        |> Option.ofObj
        |> ToAsyncSeq token
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token cursor =
        cursor
        |> Option.ofObj
        |> AsyncExecute token
        |> Async.StartAsTask