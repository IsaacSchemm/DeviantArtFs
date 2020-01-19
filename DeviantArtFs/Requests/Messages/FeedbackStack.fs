namespace DeviantArtFs.Requests.Messages

open DeviantArtFs
open FSharp.Control

module FeedbackStack =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (stackid: string) = async {
        let query = seq {
            yield! QueryFor.paging paging 50
        }

        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/messages/feedback/%s?%s" (Dafs.urlEncode stackid)
            |> Dafs.createRequest token

        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtMessage>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask