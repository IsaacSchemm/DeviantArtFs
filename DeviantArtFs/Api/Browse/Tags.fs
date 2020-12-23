namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Tags =
    let AsyncExecute token common paging (tag: string) = async {
        let query = seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtBrowsePagedResult.Parse json
    }

    let ToAsyncSeq token common offset tag =
        Dafs.getMax (AsyncExecute token common)
        |> Dafs.toAsyncSeq offset tag

    let ToArrayAsync token common offset limit tag =
        ToAsyncSeq token common offset tag
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging tag =
        AsyncExecute token common paging tag
        |> Async.StartAsTask