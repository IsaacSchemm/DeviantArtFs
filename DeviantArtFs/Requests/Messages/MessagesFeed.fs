namespace DeviantArtFs

open System

type MessagesFeedRequest() =
    member val Folderid = Nullable<Guid>()
    member val Stack = true
    member val Cursor = null

module MessagesFeed =
    open System.IO

    let AsyncExecute token (req: MessagesFeedRequest) = async {
        let query = seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            if req.Cursor <> null then
                yield sprintf "cursor=%s" req.Cursor
        }

        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feed?%s"
            |> dafs.createRequest token

        let! json = dafs.asyncRead req
        return DeviantArtFeedCursorResult<DeviantArtMessage>.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.mapFeedCursorResult (fun o -> o :> IBclDeviantArtMessage) |> Async.StartAsTask