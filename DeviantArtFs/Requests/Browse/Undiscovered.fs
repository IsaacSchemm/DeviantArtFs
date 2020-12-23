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
            yield! QueryFor.paging paging 120
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/undiscovered?%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit tag =
        ToAsyncSeq token offset tag
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask