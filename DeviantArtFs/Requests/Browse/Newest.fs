namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop

type NewestRequest() =
    member val CategoryPath = null with get, set
    member val Q = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Newest =
    let AsyncExecute token (req: NewestRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/newest?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseSearchList DeviationResponse.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMapResult Deviation |> Async.StartAsTask