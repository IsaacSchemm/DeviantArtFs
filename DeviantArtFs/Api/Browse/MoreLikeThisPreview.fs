namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs

module MoreLikeThisPreview =
    let AsyncExecute token (seed: Guid) =
        seq {
            yield sprintf "seed=%O" seed
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMoreLikeThisPreviewResult>

    let ExecuteAsync token seed = AsyncExecute token seed |> Async.StartAsTask