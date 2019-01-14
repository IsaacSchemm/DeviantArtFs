namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 120 with get, set
    member val ExtParams = new ExtParams() with get, set

module Delta =
    open FSharp.Control

    let AsyncExecute token (req: DeltaRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
            yield sprintf "ext_submission=%b" req.ExtParams.ExtSubmission
            yield sprintf "ext_camera=%b" req.ExtParams.ExtCamera
            yield sprintf "ext_stats=%b" req.ExtParams.ExtStats
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = DeltaResponse.Parse json
        return StashDeltaResult resp
    }

    let GetAll token (extParams: ExtParams) = asyncSeq {
        let mutable offset = 0
        let mutable has_more = true
        while has_more do
            let! resp = new DeltaRequest(ExtParams = extParams, Offset = offset, Limit = 120) |> AsyncExecute token
            for e in resp.Entries do
                yield e
            offset <- resp.NextOffset |> Option.defaultValue 0
            has_more <- resp.HasMore
    }

    let GetAllAsArrayAsync token extParams =
        GetAll token extParams
        |> AsyncSeq.map (fun x -> x :> IBclStashDeltaEntry)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenTo (fun x -> x :> IBclStashDeltaResult) |> Async.StartAsTask