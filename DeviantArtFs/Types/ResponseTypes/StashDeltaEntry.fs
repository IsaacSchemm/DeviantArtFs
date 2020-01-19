namespace DeviantArtFs

type StashDeltaEntry = {
    itemid: int64 option
    stackid: int64 option
    metadata: StashMetadata option
    position: int option
} with
    member this.GetItemIdOrNull() = Option.toNullable this.itemid
    member this.GetStackIdOrNull() = Option.toNullable this.stackid
    member this.GetPositionOrNull() = Option.toNullable this.position
    member this.GetMetadata() = OptUtils.toSeq this.metadata