namespace DeviantArtFs.Deviation

open DeviantArtFs
open FSharp.Data
open System

type EmbeddedContentResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 1,
    "has_less": false,
    "prev_offset": null,
    "results": []
},
{
    "has_more": false,
    "next_offset": null,
    "has_less": true,
    "prev_offset": 1,
    "results": []
}
]""", SampleIsList=true>

type EmbeddedContentResult = {
    HasMore: bool
    NextOffset: int option
    HasLess: bool
    PrevOffset: int option
    Results: seq<DeviationResponse.Root>
} with
    member this.GetNextOffset() = this.NextOffset |> Option.toNullable
    member this.GetPrevOffset() = this.PrevOffset |> Option.toNullable

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
        let resp = EmbeddedContentResponse.Parse json
        return {
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            HasLess = resp.HasLess
            PrevOffset = resp.PrevOffset
            Results = resp.Results |> Seq.map (fun j -> j.JsonValue.ToString()) |> Seq.map DeviationResponse.Parse
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask