namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop

module Tags =
    let AsyncExecute token (tag: string) (paging: PagingParams) = async {
        let query = seq {
            yield sprintf "tag=%s" (dafs.urlEncode tag)
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage DeviationResponse.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token req |> dafs.toAsyncSeq offset

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenMapResult Deviation |> Async.StartAsTask