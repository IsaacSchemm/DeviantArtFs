namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs

module MoreLikeThisPreview =
    let AsyncExecute token (seed: Guid) = async {
        let query = seq {
            yield sprintf "seed=%O" seed
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtMoreLikeThisPreviewResult.Parse json
    }

    let ExecuteAsync token seed = AsyncExecute token seed |> AsyncThen.map (fun o -> o :> IBclDeviantArtMoreLikeThisPreviewResult) |> Async.StartAsTask