namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System
open FSharp.Control

type EmbeddedContentRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val OffsetDeviationid = Nullable<Guid>() with get, set

module EmbeddedContent =
    let AsyncExecute token (req: EmbeddedContentRequest) paging =
        seq {
            yield sprintf "deviationid=%O" req.Deviationid
            match Option.ofNullable req.OffsetDeviationid with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtEmbeddedContentPagedResult>

    let ToAsyncSeq token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token req)

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> Async.StartAsTask