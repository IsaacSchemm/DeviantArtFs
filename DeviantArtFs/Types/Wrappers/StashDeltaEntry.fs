namespace DeviantArtFs

open System

type ISerializableStashDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member MetadataJson: string
    abstract member Position: Nullable<int>

type IBclStashDeltaEntry =
    inherit ISerializableStashDeltaEntry
    abstract member Metadata: IBclStashMetadata

type StashDeltaEntry = {
    itemid: int64 option
    stackid: int64 option
    metadata: StashMetadata option
    position: int option
} with
    interface IBclStashDeltaEntry with
        member this.Itemid = this.itemid |> Option.toNullable
        member this.Stackid = this.stackid |> Option.toNullable
        member this.MetadataJson = this.metadata |> Option.map (fun m -> m.Json) |> Option.toObj
        member this.Position = this.position |> Option.toNullable
        member this.Metadata = this.metadata |> Option.map (fun m -> m :> IBclStashMetadata) |> Option.toObj
