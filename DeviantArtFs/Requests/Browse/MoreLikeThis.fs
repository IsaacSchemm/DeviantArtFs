namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs
open FSharp.Control

type MoreLikeThisRequest(seed: Guid) = 
    member __.Seed = seed
    member val Category = null with get, set

module MoreLikeThis =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: MoreLikeThisRequest) = async {
        let query = seq {
            yield sprintf "seed=%O" req.Seed
            match Option.ofObj req.Category with
            | Some s -> yield sprintf "category=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<Deviation>.Parse
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