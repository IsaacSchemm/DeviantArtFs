namespace DeviantArtFs.ResponseTypes

open System

type CollectionFolder = {
    folderid: Guid
    name: string
    size: int option
    deviations: Deviation list option
}