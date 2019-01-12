namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs
open DeviantArtFs.Interop
    open FSharp.Control

type MoreLikeThisRequest(seed: Guid) = 
    member __.Seed = seed
    member val Category = null with get, set

module MoreLikeThis =
    let AsyncExecute token (paging: PagingParams) (req: MoreLikeThisRequest) = async {
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
        return json |> dafs.parsePage (DeviationResponse.Parse >> Deviation)
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenMapResult dafs.asBclDeviation |> Async.StartAsTask