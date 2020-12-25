namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module DeviantsYouWatch =
    let AsyncExecute token common paging = async {
        let query = seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let AsyncGetPage token common limit offset =
        AsyncExecute token common { Offset = offset; Limit = limit }

    let ToAsyncSeq token common offset =
        Dafs.toAsyncSeq3 offset (AsyncGetPage token common DeviantArtPagingParams.Max)

    let ToArrayAsync token common offset limit =
        ToAsyncSeq token common offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging =
        AsyncExecute token common paging
        |> Async.StartAsTask