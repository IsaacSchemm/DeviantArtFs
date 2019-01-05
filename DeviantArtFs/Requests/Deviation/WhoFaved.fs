namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data
open System

type internal WhoFavedElement = JsonProvider<"""{
    "user": {},
    "time": 2222222222
}""">

type WhoFavedUser = {
    User: DeviantArtUser
    Time: int64
}

type WhoFavedRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module WhoFaved =
    let AsyncExecute token (req: WhoFavedRequest) = async {
        let query = seq {
            yield sprintf "deviationid=%O" req.Deviationid
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage (fun j ->
            let w = WhoFavedElement.Parse j
            {
                User = w.User.JsonValue.ToString() |> dafs.parseUser
                Time = w.Time
            })
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenCastResult |> Async.StartAsTask