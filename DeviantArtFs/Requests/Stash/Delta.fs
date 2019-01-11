namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal DeltaResponse = JsonProvider<"""[{
    "cursor": "cursor1",
    "has_more": true,
    "next_offset": 42,
    "reset": false,
    "entries": [
        {
            "stackid": 1555555555555555,
            "metadata": {},
            "position": 40
        },
        {
            "itemid": 5555555555555555,
            "stackid": 88888888888880,
            "metadata": {},
            "position": 0
        },
        {
            "itemid": 7777777777777777,
            "stackid": null,
            "metadata": null
        }
    ]
}, {
    "cursor": "cursor2",
    "has_more": false,
    "next_offset": null,
    "reset": false,
    "entries": []
}]""", SampleIsList=true>

type DeltaResultEntry = {
    Itemid: int64 option
    Stackid: int64 option
    Metadata: StashMetadataResponse.Root option
    Position: int option
}
with
    interface IDeltaEntry with
        member this.Itemid = this.Itemid |> Option.toNullable
        member this.Stackid = this.Stackid |> Option.toNullable
        member this.MetadataJson = this.Metadata |> Option.map (fun j -> j.JsonValue.ToString()) |> Option.toObj
        member this.Position = this.Position |> Option.toNullable

type DeltaResult = {
    Cursor: string
    HasMore: bool
    NextOffset: int option
    Reset: bool
    Entries: seq<DeltaResultEntry>
}
with
    interface IDeltaResult with
        member this.Cursor = this.Cursor
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.Reset = this.Reset
        member this.Entries = this.Entries |> Seq.map DeltaEntry

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 120 with get, set
    member val ExtParams = new ExtParams() with get, set

module Delta =
    open FSharp.Control

    let AsyncExecute token (req: DeltaRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
            yield sprintf "ext_submission=%b" req.ExtParams.ExtSubmission
            yield sprintf "ext_camera=%b" req.ExtParams.ExtCamera
            yield sprintf "ext_stats=%b" req.ExtParams.ExtStats
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = DeltaResponse.Parse json
        return {
            Cursor = resp.Cursor
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            Reset = resp.Reset
            Entries = seq {
                for e in resp.Entries do
                    yield {
                        Itemid = e.Itemid
                        Stackid = e.Stackid
                        Metadata = e.Metadata |> Option.map (fun j -> j.JsonValue.ToString() |> StashMetadataResponse.Parse)
                        Position = e.Position
                    }
            }
        }
    }

    let GetAll token (extParams: ExtParams) = asyncSeq {
        let mutable offset = 0
        let mutable has_more = true
        while has_more do
            let! resp = new DeltaRequest(ExtParams = extParams, Offset = offset, Limit = 120) |> AsyncExecute token
            for e in resp.Entries do
                yield e
            offset <- resp.NextOffset |> Option.defaultValue 0
            has_more <- resp.HasMore
    }

    let GetAllAsListAsync token extParams =
        GetAll token extParams
        |> AsyncSeq.toListAsync
        |> iop.thenMap DeltaEntry
        |> Async.StartAsTask

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenTo (fun x -> x :> IDeltaResult) |> Async.StartAsTask