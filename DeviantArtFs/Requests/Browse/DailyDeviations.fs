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
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseListOnly (DeviationResponse.Parse >> Deviation) json
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMap dafs.asBclDeviation |> Async.StartAsTask