namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs

type DailyDeviationsRequest() = 
    member val Date = Nullable<DateTime>() with get, set

module DailyDeviations =
    let AsyncExecute token (req: DailyDeviationsRequest) = async {
        let query = seq {
            match Option.ofNullable req.Date with
            | Some d -> yield d.ToString("YYYY-MM-dd") |> sprintf "date=%s"
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.mapSeq (fun o -> o :> IBclDeviation) |> Async.StartAsTask