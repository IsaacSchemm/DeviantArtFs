namespace DeviantArtFs.Requests.User

open DeviantArtFs
open DeviantArtFs.Interop

module StatusesList =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (username: string) = async {
        let query = seq {
            yield sprintf "username=%s" (dafs.urlEncode username)
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage (StatusResponse.Parse >> Status) json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToListAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toListAsync
        |> iop.thenMap (fun s -> s :> IBclStatus) 
        |> Async.StartAsTask

    let ExecuteAsync token paging username = AsyncExecute token paging username |> iop.thenMapResult (fun s -> s :> IBclStatus)  |> Async.StartAsTask