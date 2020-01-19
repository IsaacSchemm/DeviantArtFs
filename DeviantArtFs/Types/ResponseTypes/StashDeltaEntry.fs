namespace DeviantArtFs

type StashDeltaEntry = {
    itemid: int64 option
    stackid: int64 option
    metadata: StashMetadata option
    position: int option
} with
    member this.GetItemId() = OptUtils.toNullable this.itemid
    member this.GetStackId() = OptUtils.toNullable this.stackid
    member this.GetPosition() = OptUtils.toNullable this.position
    member this.GetMetadata() = OptUtils.toSeq this.metadata