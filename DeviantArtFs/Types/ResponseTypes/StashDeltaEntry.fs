namespace DeviantArtFs

type StashDeltaEntry = {
    itemid: int64 option
    stackid: int64 option
    metadata: StashMetadata option
    position: int option
}