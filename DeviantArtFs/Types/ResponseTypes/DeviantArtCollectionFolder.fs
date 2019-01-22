namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtCollectionFolder =
    abstract member Folderid: Guid
    abstract member Name: string
    abstract member Size: Nullable<int>

type DeviantArtCollectionFolder = {
    folderid: Guid
    name: string
    size: int option
} with
    static member Parse json = Json.deserialize<DeviantArtCollectionFolder> json
    interface IBclDeviantArtCollectionFolder with
        member this.Folderid = this.folderid
        member this.Name = this.name
        member this.Size = this.size |> Option.toNullable