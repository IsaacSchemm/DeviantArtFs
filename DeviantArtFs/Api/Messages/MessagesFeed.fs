namespace DeviantArtFs.Api.Messages

open System
open DeviantArtFs
open FSharp.Control

type MessagesFeedRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Stack = true with get, set

module MessagesFeed =
    let AsyncExecute token common (cursor: string option) (req: MessagesFeedRequest) =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            match cursor with
            | Some c -> yield sprintf "cursor=%s" c
            | None -> ()
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/messages/feed"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMessageCursorResult>

    let AsyncGetPage token common req cursor =
        AsyncExecute token common cursor req

    let ToAsyncSeq token common cursor req =
        Dafs.toAsyncSeq3 cursor (AsyncGetPage token common req)

    let ToArrayAsync token common req cursor limit =
        ToAsyncSeq token common (Option.ofObj cursor) req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common cursor req =
        AsyncExecute token common (Option.ofObj cursor) req
        |> Async.StartAsTask