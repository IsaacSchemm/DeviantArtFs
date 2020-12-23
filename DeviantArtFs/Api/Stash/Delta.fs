namespace DeviantArtFs.Api.Stash

open DeviantArtFs

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val ExtParams = DeviantArtExtParams.None with get, set

module Delta =
    open FSharp.Control

    let AsyncExecute token paging (req: DeltaRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.extParams req.ExtParams
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return StashDeltaResult.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask