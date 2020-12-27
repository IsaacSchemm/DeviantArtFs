namespace DeviantArtFs

open System

type DeviantArtGalleryFolder = {
    folderid: Guid
    parent: Guid option
    name: string
    size: int option
    deviations: Deviation list option
}