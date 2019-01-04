namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs
open DeviantArtFs.Interop

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
        let o = ListOnlyResponse.Parse json
        return seq {
            for element in o.Results do
                let json = element.JsonValue.ToString()
                yield json |> DeviationResponse.Parse
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> dafs.whenDone (Seq.map Deviation) |> Async.StartAsTask