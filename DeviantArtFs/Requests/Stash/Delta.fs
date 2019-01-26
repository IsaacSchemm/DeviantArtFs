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

    let GetAll token req offset = AsyncExecute token |> Dafs.toAsyncSeq offset 120 req

    let GetAllAsArrayAsync token extParams =
        GetAll token (new DeltaRequest(ExtParams = extParams)) 0
        |> AsyncSeq.map (fun x -> x :> IBclStashDeltaEntry)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> AsyncThen.map (fun x -> x :> IBclStashDeltaResult) |> Async.StartAsTask