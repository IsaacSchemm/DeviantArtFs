namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtGalleryFolder =
    abstract member Folderid: Guid
    abstract member Parent: Nullable<Guid>
    abstract member Name: string
    abstract member Size: Nullable<int>
    abstract member Deviations: seq<IBclDeviation>

type DeviantArtGalleryFolder = {
    folderid: Guid
    parent: Guid option
    name: string
    size: int option
    deviations: Deviation[] option
} with
    static member Parse json = Json.deserialize<DeviantArtGalleryFolder> json
    interface IBclDeviantArtGalleryFolder with
        member this.Folderid = this.folderid
        member this.Parent = this.parent |> Option.toNullable
        member this.Name = this.name
        member this.Size = this.size |> Option.toNullable
        member this.Deviations = this.deviations |> Option.map Seq.ofArray |> Option.defaultValue Seq.empty |> Seq.map (fun d -> d :> IBclDeviation)