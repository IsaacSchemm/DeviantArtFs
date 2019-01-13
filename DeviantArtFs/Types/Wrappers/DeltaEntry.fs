namespace DeviantArtFs

open System

type ISerializedDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member MetadataJson: string
    abstract member Position: Nullable<int>

type IBclDeltaEntry =
    inherit ISerializedDeltaEntry
    abstract member Metadata: IBclStashMetadata

type DeltaEntry(original: DeltaResponse.Entry) =
    member __.Original = original

    member __.Itemid = original.Itemid
    member __.Stackid = original.Stackid
    member __.MetadataJson = original.Metadata |> Option.map (fun o -> o.JsonValue.ToString())
    member __.Position = original.Position

    member this.Metadata =
        this.MetadataJson
        |> Option.map (StashMetadataResponse.Parse >> StashMetadata)

    interface IBclDeltaEntry with
        member this.Itemid = this.Itemid |> Option.toNullable
        member this.Stackid = this.Stackid |> Option.toNullable
        member this.MetadataJson = this.MetadataJson |> Option.toObj
        member this.Metadata = this.Metadata |> Option.map (fun m -> m :> IBclStashMetadata) |> Option.toObj
        member this.Position = this.Position |> Option.toNullable