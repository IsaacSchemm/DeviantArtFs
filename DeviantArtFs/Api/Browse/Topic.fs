namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Topic =
    let AsyncExecute token common paging (topic: string) = async {
        let query = seq {
            match Option.ofObj topic with
            | Some s -> yield sprintf "topic=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.commonParams common
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/topic?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token common offset topic =
        (fun p -> AsyncExecute token common p topic)
        |> Dafs.toAsyncSeq2 offset

    let ToArrayAsync token common offset limit topic =
        ToAsyncSeq token common offset topic
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging topic =
        AsyncExecute token common paging topic
        |> Async.StartAsTask