namespace DeviantArtFs

open System

type IStashDelta =
    /// The item ID, if this object represents an item.
    abstract member Itemid: Nullable<int64>
    /// The ID of this stack, or - if this object represents an item - the item's parent stack.
    abstract member Stackid: Nullable<int64>
    /// The difference in position between the object's previous position (within its stack) and its new one.
    abstract member Position: Nullable<int>

/// A Sta.sh delta entry.
type IBclStashDeltaEntry =
    inherit IStashDelta
    /// Metadata about the stack or item.
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
        member this.Position = this.position |> Option.toNullable
        member this.Metadata = this.metadata |> Option.map (fun m -> m :> IBclStashMetadata) |> Option.toObj
