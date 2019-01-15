namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtCollectionFolder =
    abstract member Folderid: Guid
    abstract member Name: string
    abstract member Size: Nullable<int>

type DeviantArtCollectionFolder = {
    Folderid: Guid
    Name: string
    Size: int option
} with
    static member Parse json = Json.deserialize<DeviantArtCollectionFolder> json
    interface IBclDeviantArtCollectionFolder with
        member this.Folderid = this.Folderid
        member this.Name = this.Name
        member this.Size = this.Size |> Option.toNullable