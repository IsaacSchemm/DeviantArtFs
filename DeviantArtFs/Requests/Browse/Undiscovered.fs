namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

type UndiscoveredRequest() =
    member val CategoryPath = null with get, set

module Undiscovered =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: UndiscoveredRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/undiscovered?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token offset req = AsyncExecute token |> Dafs.toAsyncSeq offset 120 req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req|> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation) |> Async.StartAsTask
