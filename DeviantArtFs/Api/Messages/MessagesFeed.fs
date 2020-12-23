namespace DeviantArtFs.Api.Messages

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
            |> Dafs.createRequest token DeviantArtCommonParams.Default

        let! json = Dafs.asyncRead req
        return DeviantArtMessageCursorResult.Parse json
    }

    let ToAsyncSeq token cursor req =
        AsyncExecute token
        |> Dafs.toAsyncSeq cursor req

    let ToArrayAsync token req cursor limit =
        ToAsyncSeq token (Option.ofObj cursor) req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token cursor req =
        AsyncExecute token (Option.ofObj cursor) req
        |> Async.StartAsTask