namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs
open FSharp.Control

type MoreLikeThisRequest(seed: Guid) = 
    member __.Seed = seed
    member val Category = null with get, set

module MoreLikeThis =
    let AsyncExecute token paging (req: MoreLikeThisRequest) = async {
        let query = seq {
            yield sprintf "seed=%O" req.Seed
            match Option.ofObj req.Category with
            | Some s -> yield sprintf "category=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis?%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return json |> DeviantArtPagedResult<Deviation>.Parse
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask