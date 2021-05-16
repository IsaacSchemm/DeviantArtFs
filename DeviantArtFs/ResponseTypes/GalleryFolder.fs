namespace DeviantArtFs.ResponseTypes

open System

type GalleryFolder = {
    folderid: Guid
    parent: Guid option
    name: string
    size: int option
    deviations: Deviation list option
}