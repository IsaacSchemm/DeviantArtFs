namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module Tags =
    let AsyncExecute token expansion (tag: string) paging =
        seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let ToAsyncSeq token expansion tag offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion tag)

    let ToArrayAsync token expansion tag offset limit =
        ToAsyncSeq token expansion tag offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion tag paging =
        AsyncExecute token expansion tag paging
        |> Async.StartAsTask