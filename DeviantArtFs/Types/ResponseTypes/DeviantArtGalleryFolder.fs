namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtGalleryFolder = {
    folderid: Guid
    parent: Guid option
    name: string
    size: int option
    deviations: Deviation list option
} with
    static member Parse json = Json.deserialize<DeviantArtGalleryFolder> json
    member this.GetParent() = OptUtils.toNullable this.parent
    member this.GetSize() = OptUtils.toNullable this.size
    member this.GetDeviations() = OptUtils.listDefault this.deviations