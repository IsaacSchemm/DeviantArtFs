namespace DeviantArtFs.Requests.Browse

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
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/user/journals?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<Deviation>.Parse json
    }

    let AsyncGetMax token offset req =
        let paging = Dafs.page offset 50
        AsyncExecute token paging req

    let ToAsyncSeq token offset req =
        AsyncGetMax token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun o -> o :> IBclDeviation)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation)
        |> Async.StartAsTask

    let GetMaxAsync token paging req =
        AsyncGetMax token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviation)
        |> Async.StartAsTask