namespace DeviantArtFs.ResponseTypes

open System

type CollectionFolder = {
    folderid: Guid
    name: string
    description: string option
    size: int option
    thumb: Deviation option
    deviations: Deviation list option
}