namespace DeviantArtFs.Api.Messages

open System
open DeviantArtFs
open FSharp.Control

type MentionsMessagesRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Stack = true with get, set

module MentionsMessages =
    let AsyncExecute token common paging (req: MentionsMessagesRequest) =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/messages/mentions"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let private AsyncGetPage token common req cursor =
        AsyncExecute token common { Offset = cursor; Limit = Nullable Int32.MaxValue } req

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (AsyncGetPage token common req)

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask