namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs

type DailyDeviationsRequest() = 
    member val Date = Nullable<DateTime>() with get, set

module DailyDeviations =
    let AsyncExecute token expansion (req: DailyDeviationsRequest) =
        seq {
            match Option.ofNullable req.Date with
            | Some d -> yield d.ToString("YYYY-MM-dd") |> sprintf "date=%s"
            | None -> ()
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<Deviation>>

    let ExecuteAsync token expansion req = AsyncExecute token expansion req |> Async.StartAsTask