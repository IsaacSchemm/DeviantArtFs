namespace DeviantArtFs.Api

open DeviantArtFs
open System

module Deviation =
    let AsyncGet token expansion (id: Guid) =
        seq {
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<Deviation>

    let AsyncGetContent token (deviationid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/content?deviationid=%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationTextContent>

    let AsyncDownload token (deviationid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationDownload>

    type EmbeddedContentRequest(deviationid: Guid) =
        member __.Deviationid = deviationid
        member val OffsetDeviationid = Nullable<Guid>() with get, set

    let AsyncPageEmbeddedContent token (req: EmbeddedContentRequest) paging =
        seq {
            yield sprintf "deviationid=%O" req.Deviationid
            match Option.ofNullable req.OffsetDeviationid with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtEmbeddedContentPagedResult>

    let AsyncGetEmbeddedContent token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageEmbeddedContent token req)

    type MetadataRequest(deviationids: seq<Guid>) =
        member __.Deviationids = deviationids
        member val ExtParams = ParameterTypes.ExtParams.None with get, set
        member val ExtCollection = false with get, set

    let AsyncGetMetadata token (req: MetadataRequest) =
        seq {
            yield! QueryFor.extParams req.ExtParams
            yield sprintf "ext_collection=%b" req.ExtCollection
            for id in req.Deviationids do
                yield sprintf "deviationids[]=%O" id
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/metadata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationMetadataResponse>

    let AsyncPageWhoFaved token expansion (deviationid: Guid) paging =
        seq {
            yield sprintf "deviationid=%O" deviationid
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtWhoFavedUser>>

    let AsyncGetWhoFaved token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageWhoFaved token expansion req)