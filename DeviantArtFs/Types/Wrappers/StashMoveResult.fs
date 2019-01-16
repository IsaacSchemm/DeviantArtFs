namespace DeviantArtFs

open FSharp.Json

type IBclStashMoveResult =
    abstract member Target: IBclStashMetadata
    abstract member Changes: seq<IBclStashMetadata>

type StashMoveResult = {
    target: StashMetadata
    changes: StashMetadata[]
} with
    static member Parse json = Json.deserialize<StashMoveResult> json
    interface IBclStashMoveResult with
        member this.Target = this.target :> IBclStashMetadata
        member this.Changes = this.changes |> Seq.map (fun c -> c :> IBclStashMetadata)