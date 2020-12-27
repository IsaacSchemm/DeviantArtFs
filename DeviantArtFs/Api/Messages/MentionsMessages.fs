namespace DeviantArtFs.Api.Messages

open System
open DeviantArtFs
open FSharp.Control

type MentionsMessagesRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Stack = true with get, set

module MentionsMessages =
    let AsyncExecute token (req: MentionsMessagesRequest) paging =
        seq {
            if req.Folderid.HasValue then
                yield sprintf "folderid=%O" req.Folderid
            yield sprintf "stack=%b" req.Stack
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/messages/mentions"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtMessage>>

    let ToAsyncSeq token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token req)

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> Async.StartAsTask