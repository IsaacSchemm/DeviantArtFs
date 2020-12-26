namespace DeviantArtFs.Api.Browse

open System
open DeviantArtFs

module MoreLikeThisPreview =
    let AsyncExecute token expansion (seed: Guid) =
        seq {
            yield sprintf "seed=%O" seed
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMoreLikeThisPreviewResult>

    let ExecuteAsync token expansion seed =
        AsyncExecute token expansion seed
        |> Async.StartAsTask