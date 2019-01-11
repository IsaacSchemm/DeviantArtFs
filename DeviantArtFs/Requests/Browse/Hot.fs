namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Control

type HotRequest() = 
    member val CategoryPath = null with get, set

module Hot =
    let AsyncExecute token (paging: PagingParams) (req: HotRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/hot?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage DeviationResponse.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 120 req

    let ExecuteAsync token paging req = AsyncExecute token paging req |> iop.thenMapResult Deviation |> Async.StartAsTask