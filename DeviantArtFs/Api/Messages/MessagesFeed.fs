namespace DeviantArtFs.Api.Messages

open System
open DeviantArtFs
open FSharp.Control

type MessagesFeedRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Stack = true with get, set

module MessagesFeed =
    let AsyncExecute token common (req: MessagesFeedRequest) (cursor: string option) =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            match cursor with
            | Some c -> yield sprintf "cursor=%s" c
            | None -> ()
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/messages/feed"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMessageCursorResult>

    let ToAsyncSeq token common req cursor =
        Dafs.toAsyncSeq cursor (AsyncExecute token common req)

    let ToArrayAsync token common req cursor limit =
        ToAsyncSeq token common req (Option.ofObj cursor)
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req cursor =
        AsyncExecute token common req (Option.ofObj cursor)
        |> Async.StartAsTask