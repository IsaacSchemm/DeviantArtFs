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

    let ToAsyncSeq token offset req = AsyncExecute token |> Dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req = AsyncExecute token paging req |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation) |> Async.StartAsTask
