namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

module WhoFaved =
    open FSharp.Control

    let internal epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)

    let AsyncExecute token (paging: IDeviantArtPagingParams) (deviationid: Guid) = async {
        let query = seq {
            yield sprintf "deviationid=%O" deviationid
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<DeviantArtWhoFavedUser>.Parse
    }

    let ToAsyncSeq token offset deviationid = AsyncExecute token |> Dafs.toAsyncSeq offset 50 deviationid

    let ToArrayAsync token offset limit deviationid =
        ToAsyncSeq token offset deviationid
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun w -> w :> IBclDeviantArtWhoFavedUser)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging deviationid = AsyncExecute token paging deviationid |> AsyncThen.mapPagedResult (fun w -> w :> IBclDeviantArtWhoFavedUser) |> Async.StartAsTask