namespace DeviantArtFs

open System

type DeviantArtCollectionFolder = {
    folderid: Guid
    name: string
    size: int option
    deviations: Deviation list option
}