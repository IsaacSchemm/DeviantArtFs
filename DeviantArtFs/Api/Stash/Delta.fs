namespace DeviantArtFs.Api.Stash

open DeviantArtFs
open FSharp.Control

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val ExtParams = DeviantArtExtParams.None with get, set

module Delta =
    let AsyncExecute token common (req: DeltaRequest) paging =
        seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.extParams req.ExtParams
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashDeltaResult>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq3 (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common req)

    let ToArrayAsync token common req offset limit =
        ToAsyncSeq token common req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask