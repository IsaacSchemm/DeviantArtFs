namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

module Tags =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (tag: string) = async {
        let query = seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let AsyncGetMax token offset tag =
        let paging = Dafs.page offset 50
        AsyncExecute token paging tag

    let ToAsyncSeq token offset tag =
        AsyncGetMax token
        |> Dafs.toAsyncSeq offset tag

    let ToArrayAsync token offset limit tag =
        ToAsyncSeq token offset tag
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging tag =
        AsyncExecute token paging tag
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation)
        |> Async.StartAsTask

    let GetMaxAsync token paging tag =
        AsyncGetMax token paging tag
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation)
        |> Async.StartAsTask