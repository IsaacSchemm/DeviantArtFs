namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

type HotRequest() = 
    member val CategoryPath = null with get, set

module Hot =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: HotRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/hot?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<Deviation>.Parse
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation)
        |> Async.StartAsTask