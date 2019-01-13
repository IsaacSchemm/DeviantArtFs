namespace DeviantArtFs

open System

type ISerializedStashDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member MetadataJson: string
    abstract member Position: Nullable<int>

type IBclStashDeltaEntry =
    inherit ISerializedStashDeltaEntry
    abstract member Metadata: IBclStashMetadata

type StashDeltaEntry(serialized: ISerializedStashDeltaEntry) =
    new(original: DeltaResponse.Entry) =
        StashDeltaEntry({
            new ISerializedStashDeltaEntry with
                member __.Itemid = original.Itemid |> Option.toNullable
                member __.Stackid = original.Stackid |> Option.toNullable
                member __.MetadataJson = original.Metadata |> Option.map (fun j -> j.JsonValue.ToString()) |> Option.toObj
                member __.Position = original.Position |> Option.toNullable
        })

    member __.Itemid = serialized.Itemid |> Option.ofNullable
    member __.Stackid = serialized.Stackid |> Option.ofNullable
    member __.MetadataJson = serialized.MetadataJson |> Option.ofObj
    member __.Position = serialized.Position |> Option.ofNullable

    member this.Metadata =
        this.MetadataJson
        |> Option.map (StashMetadataResponse.Parse >> StashMetadata)

    interface IBclStashDeltaEntry with
        member __.Itemid = serialized.Itemid
        member __.Stackid = serialized.Stackid
        member __.MetadataJson = serialized.MetadataJson
        member __.Position = serialized.Position
        member this.Metadata =
            this.Metadata
            |> Option.map (fun m -> m :> IBclStashMetadata)
            |> Option.toObj