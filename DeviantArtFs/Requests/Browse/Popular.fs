namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Control

type PopularTimeRange =
| EightHours = 1
| TwentyFourHours = 2
| ThreeDays = 3
| OneWeek = 4
| OneMonth = 5
| AllTime = 6

type PopularRequest() =
    member val CategoryPath = null with get, set
    member val Q = null with get, set
    member val Timerange = PopularTimeRange.TwentyFourHours with get, set

module Popular =
    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: PopularRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (Dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            match req.Timerange with
            | PopularTimeRange.EightHours -> yield "timerange=8hr"
            | PopularTimeRange.TwentyFourHours -> yield "timerange=24hr"
            | PopularTimeRange.ThreeDays -> yield "timerange=3days"
            | PopularTimeRange.OneWeek -> yield "timerange=1week"
            | PopularTimeRange.OneMonth -> yield "timerange=1month"
            | PopularTimeRange.AllTime -> yield "timerange=alltime"
            | _ -> ()
            yield! QueryFor.paging paging 120
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/popular?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtBrowsePagedResult.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask