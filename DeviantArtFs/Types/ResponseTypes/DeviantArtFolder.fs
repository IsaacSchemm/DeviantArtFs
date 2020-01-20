namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtFolder = {
    folderid: Guid
    parent: Guid option
    name: string
    size: int option
    deviations: Deviation list option
} with
    static member Parse json = Json.deserialize<DeviantArtFolder> json
    member this.GetParent() = OptUtils.guidDefault this.parent
    member this.GetSize() = OptUtils.intDefault this.size
    member this.GetDeviations() = OptUtils.listDefault this.deviations