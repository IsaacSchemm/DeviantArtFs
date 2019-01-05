namespace DeviantArtFs.Requests.Gallery

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type AllRequest() =
    member val Username = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module All =
    let AsyncExecute token (req: AllRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/all?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseGenericList DeviationResponse.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMapResult Deviation |> Async.StartAsTask