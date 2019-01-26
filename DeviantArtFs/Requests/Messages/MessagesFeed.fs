namespace DeviantArtFs.Requests.Messages

open System
open DeviantArtFs
open FSharp.Control

type MessagesFeedRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Stack = true with get, set

module MessagesFeed =
    let AsyncExecute token (cursor: string option) (req: MessagesFeedRequest) = async {
        let query = seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            match cursor with
            | Some c -> yield sprintf "cursor=%s" c
            | None -> ()
        }

        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feed?%s"
            |> Dafs.createRequest token

        let! json = Dafs.asyncRead req
        return DeviantArtMessageCursorResult<DeviantArtMessage>.Parse json
    }

    let ToAsyncSeq token req cursor =
        let curried c = AsyncExecute token c req
        curried |> Dafs.cursorToAsyncSeq cursor

    let ToArrayAsync token req cursor limit =
        cursor
        |> Option.ofObj
        |> ToAsyncSeq token req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviantArtMessage)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token cursor req = AsyncExecute token (Option.ofObj cursor) req |> AsyncThen.mapMessageCursorResult (fun o -> o :> IBclDeviantArtMessage) |> Async.StartAsTask