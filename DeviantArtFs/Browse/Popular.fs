namespace DeviantArtFs.Browse

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
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Popular =
    let AsyncExecute token (req: PopularRequest) = async {
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
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/popular?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = GenericListResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            EstimatedTotal = o.EstimatedTotal
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield DeviationResponse.Parse json
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask