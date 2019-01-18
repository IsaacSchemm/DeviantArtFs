namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

type NewestRequest() =
    member val CategoryPath = null with get, set
    member val Q = null with get, set

module Newest =
    let AsyncExecute token (paging: PagingParams) (req: NewestRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/newest?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage Deviation.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 120 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map dafs.asBclDeviation
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> iop.thenMapResult dafs.asBclDeviation |> Async.StartAsTask