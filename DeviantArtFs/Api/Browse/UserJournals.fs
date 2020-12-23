namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

type UserJournalsRequest(username: string) =
    member __.Username = username
    member val Featured = true with get, set

module UserJournals =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: UserJournalsRequest) = async {
        let query = seq {
            yield sprintf "username=%s" (Dafs.urlEncode req.Username)
            yield sprintf "featured=%b" req.Featured
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/user/journals?%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask