namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System
open FSharp.Control

type EmbeddedContentRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val OffsetDeviationid = Nullable<Guid>() with get, set

module EmbeddedContent =
    let AsyncExecute token common (req: EmbeddedContentRequest) paging =
        seq {
            yield sprintf "deviationid=%O" req.Deviationid
            match Option.ofNullable req.OffsetDeviationid with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtEmbeddedContentPagedResult>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq3 (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token common req)

    let ToArrayAsync token common req offset limit =
        ToAsyncSeq token common req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask