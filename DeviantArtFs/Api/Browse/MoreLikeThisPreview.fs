namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs

module MoreLikeThisPreview =
    let AsyncExecute token common (seed: Guid) =
        seq {
            yield sprintf "seed=%O" seed
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMoreLikeThisPreviewResult>

    let ExecuteAsync token common seed =
        AsyncExecute token common seed
        |> Async.StartAsTask