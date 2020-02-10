namespace DeviantArtFs

type StashDeltaEntry = {
    itemid: int64 option
    stackid: int64 option
    metadata: StashMetadata option
    position: int option
} with
    member this.GetItemId() = OptUtils.longDefault this.itemid
    member this.GetStackId() = OptUtils.longDefault this.stackid
    member this.GetPosition() = OptUtils.intDefault this.position
    member this.GetMetadata() = OptUtils.recordDefault this.metadata