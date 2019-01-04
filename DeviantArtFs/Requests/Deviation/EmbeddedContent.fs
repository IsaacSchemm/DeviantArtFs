namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data
open System

type EmbeddedContentRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val OffsetDeviationid = Nullable<Guid>() with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module EmbeddedContent =
    let AsyncExecute token (req: EmbeddedContentRequest) = async {
        let query = seq {
            yield sprintf "deviationid=%O" req.Deviationid
            match Option.ofNullable req.OffsetDeviationid with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = BidirectionalListResponse.Parse json
        return {
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            HasLess = resp.HasLess
            PrevOffset = resp.PrevOffset
            Results = resp.Results |> Seq.map (fun j -> j.JsonValue.ToString()) |> Seq.map DeviationResponse.Parse
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMapResult Deviation |> Async.StartAsTask