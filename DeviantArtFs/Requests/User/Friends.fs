namespace DeviantArtFs.Requests.User

open DeviantArtFs

type FriendsRequest() =
    member val Username: string = null with get, set

module Friends =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: FriendsRequest) = async {
        let query = seq {
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s?%s" (dafs.urlEncode req.Username)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage DeviantArtFriendRecord.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun w -> w :> IBclDeviantArtFriendRecord)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> iop.thenMapResult (fun w -> w :> IBclDeviantArtFriendRecord)
        |> Async.StartAsTask