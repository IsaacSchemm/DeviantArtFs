namespace DeviantArtFs.Api.Browse

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
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtMoreLikeThisPreviewResult.Parse json
    }

    let ExecuteAsync token seed = AsyncExecute token seed |> Async.StartAsTask