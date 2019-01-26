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

    let AsyncGetMax token offset req =
        let paging = Dafs.page offset 50
        AsyncExecute token paging req

    let ToAsyncSeq token offset req =
        AsyncGetMax token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit deviationid =
        ToAsyncSeq token offset deviationid
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun w -> w :> IBclDeviantArtWhoFavedUser)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtWhoFavedUser)
        |> Async.StartAsTask

    let GetMaxAsync token paging req =
        AsyncGetMax token paging req
        |> AsyncThen.mapPagedResult (fun o -> o :> IBclDeviantArtWhoFavedUser)
        |> Async.StartAsTask