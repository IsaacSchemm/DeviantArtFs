namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

type HotRequest() = 
    member val CategoryPath = null with get, set

module Hot =
    let AsyncExecute token (paging: IPagingParams) (req: HotRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            yield! queryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/hot?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> DeviantArtPagedResult<Deviation>.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 120 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation) |> Async.StartAsTask