namespace DeviantArtFs.Api.Stash

open DeviantArtFs
open FSharp.Control

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val ExtParams = DeviantArtExtParams.None with get, set

module Delta =
    let AsyncExecute token (req: DeltaRequest) paging =
        seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.extParams req.ExtParams
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashDeltaResult>

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