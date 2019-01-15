namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

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
    let AsyncExecute token (paging: PagingParams) (req: PopularRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (dafs.urlEncode s)
            | None -> ()
            match req.Timerange with
            | PopularTimeRange.EightHours -> yield "timerange=8hr"
            | PopularTimeRange.TwentyFourHours -> yield "timerange=24hr"
            | PopularTimeRange.ThreeDays -> yield "timerange=3days"
            | PopularTimeRange.OneWeek -> yield "timerange=1week"
            | PopularTimeRange.OneMonth -> yield "timerange=1month"
            | PopularTimeRange.AllTime -> yield "timerange=alltime"
            | _ -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/popular?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage Deviation.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token req |> dafs.toAsyncSeq offset 120

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenMapResult dafs.asBclDeviation |> Async.StartAsTask