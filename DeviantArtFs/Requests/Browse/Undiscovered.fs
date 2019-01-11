namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop

type UndiscoveredRequest() =
    member val CategoryPath = null with get, set

module Undiscovered =
    let AsyncExecute token (paging: PagingParams) (req: UndiscoveredRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.ToQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/undiscovered?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage DeviationResponse.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token req |> dafs.toAsyncSeq offset 120

    let ExecuteAsync token req paging = AsyncExecute token req paging|> iop.thenMapResult Deviation |> Async.StartAsTask