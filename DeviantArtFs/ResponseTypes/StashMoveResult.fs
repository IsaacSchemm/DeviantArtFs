namespace DeviantArtFs.ResponseTypes

type StashMoveResult = {
    target: StashMetadata
    changes: StashMetadata list
}