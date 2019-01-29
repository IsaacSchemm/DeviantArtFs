namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtCollectionFolder =
    abstract member Folderid: Guid
    abstract member Name: string
    abstract member Size: Nullable<int>
    abstract member Deviations: seq<IBclDeviation>

type DeviantArtCollectionFolder = {
    folderid: Guid
    name: string
    size: int option
    deviations: Deviation list option
} with
    static member Parse json = Json.deserialize<DeviantArtCollectionFolder> json
    interface IBclDeviantArtCollectionFolder with
        member this.Folderid = this.folderid
        member this.Name = this.name
        member this.Size = this.size |> Option.toNullable
        member this.Deviations = this.deviations |> Option.map Seq.ofList |> Option.defaultValue Seq.empty |> Seq.map (fun d -> d :> IBclDeviation)