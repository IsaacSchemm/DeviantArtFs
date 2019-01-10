namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs
open DeviantArtFs.Interop
    open FSharp.Control

type MoreLikeThisRequest(seed: Guid) = 
    member __.Seed = seed
    member val Category = null with get, set

module MoreLikeThis =
    let AsyncExecute token (req: MoreLikeThisRequest) (paging: PagingParams) = async {
        let query = seq {
            yield sprintf "seed=%O" req.Seed
            match Option.ofObj req.Category with
            | Some s -> yield sprintf "category=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage DeviationResponse.Parse
    }

    let ToAsyncSeq token req offset = AsyncExecute token req |> dafs.toAsyncSeq offset

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenMapResult Deviation |> Async.StartAsTask