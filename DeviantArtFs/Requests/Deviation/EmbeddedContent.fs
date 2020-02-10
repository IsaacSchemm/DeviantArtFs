namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open System

type EmbeddedContentRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val OffsetDeviationid = Nullable<Guid>() with get, set

module EmbeddedContent =
    open FSharp.Control

    let AsyncExecute token (paging: IDeviantArtPagingParams) (req: EmbeddedContentRequest) = async {
        let query = seq {
            yield sprintf "deviationid=%O" req.Deviationid
            match Option.ofNullable req.OffsetDeviationid with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtEmbeddedContentPagedResult.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax AsyncExecute token
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask