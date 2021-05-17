namespace DeviantArtFs.Api

open DeviantArtFs
open System
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

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
        |> Dafs.thenParse<TextContent>

    let AsyncDownload token (deviationid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<Download>

    type EmbeddedContentRequest(deviationid: Guid) =
        member __.Deviationid = deviationid
        member val OffsetDeviationid = Nullable<Guid>() with get, set

    let AsyncPageEmbeddedContent token deviationid offset_deviationid limit offset =
        seq {
            yield sprintf "deviationid=%O" (Dafs.guid2str deviationid)
            yield! QueryFor.embeddedDeviationOffset offset_deviationid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent"
        |> Dafs.asyncRead
        |> Dafs.thenParse<EmbeddedContentPage>

    let AsyncGetEmbeddedContent token deviationid batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageEmbeddedContent token deviationid StartWithFirstEmbeddedDeviation batchsize)

    let AsyncGetMetadata token extParams deviationids =
        seq {
            yield! QueryFor.extParams extParams
            for id in deviationids do
                yield sprintf "deviationids[]=%O" (Dafs.guid2str id)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/metadata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<MetadataResponse>

    let AsyncPageWhoFaved token expansion (deviationid: Guid) limit offset =
        seq {
            yield sprintf "deviationid=%O" deviationid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<WhoFavedUser>>

    let AsyncGetWhoFaved token expansion req batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageWhoFaved token expansion req batchsize)