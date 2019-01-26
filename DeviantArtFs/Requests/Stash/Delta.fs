namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val ExtParams = DeviantArtExtParams.None with get, set

module Delta =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: DeltaRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging
            yield sprintf "ext_submission=%b" req.ExtParams.ExtSubmission
            yield sprintf "ext_camera=%b" req.ExtParams.ExtCamera
            yield sprintf "ext_stats=%b" req.ExtParams.ExtStats
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return StashDeltaResult.Parse json
    }

    let AsyncGetMax token offset req =
        let paging = Dafs.page offset 120
        AsyncExecute token paging req

    let ToAsyncSeq token offset req =
        AsyncGetMax token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun f -> f :> IBclStashDeltaEntry)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.map (fun x -> x :> IBclStashDeltaResult)
        |> Async.StartAsTask

    let GetMaxAsync token paging req =
        AsyncGetMax token paging req
        |> AsyncThen.map (fun x -> x :> IBclStashDeltaResult)
        |> Async.StartAsTask