namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtGalleryFolder =
    abstract member Folderid: Guid
    abstract member Parent: Nullable<Guid>
    abstract member Name: string
    abstract member Size: Nullable<int>

type DeviantArtGalleryFolder = {
    Folderid: Guid
    Parent: Guid option
    Name: string
    Size: int option
} with
    static member Parse json = Json.deserialize<DeviantArtGalleryFolder> json
    interface IBclDeviantArtGalleryFolder with
        member this.Folderid = this.Folderid
        member this.Parent = this.Parent |> Option.toNullable
        member this.Name = this.Name
        member this.Size = this.Size |> Option.toNullable