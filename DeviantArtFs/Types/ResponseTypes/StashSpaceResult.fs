namespace DeviantArtFs

open FSharp.Json

type IBclStashSpaceResult =
    abstract member AvailableSpace: int64
    abstract member TotalSpace: int64

type StashSpaceResult = {
    available_space: int64
    total_space: int64
} with
    static member Parse json = Json.deserialize<StashSpaceResult> json
    interface IBclStashSpaceResult with
        member this.AvailableSpace = this.available_space
        member this.TotalSpace = this.total_space