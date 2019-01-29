namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtMoreLikeThisPreviewResult =
    abstract member Seed: Guid
    abstract member Author: IBclDeviantArtUser
    abstract member MoreFromArtist: seq<IBclDeviation>
    abstract member MoreFromDa: seq<IBclDeviation>

type DeviantArtMoreLikeThisPreviewResult = {
    seed: Guid
    author: DeviantArtUser
    more_from_artist: Deviation list
    more_from_da: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtMoreLikeThisPreviewResult> json
    interface IBclDeviantArtMoreLikeThisPreviewResult with
        member this.Seed = this.seed
        member this.Author = this.author :> IBclDeviantArtUser
        member this.MoreFromArtist = this.more_from_artist |> Seq.map (fun o -> o :> IBclDeviation)
        member this.MoreFromDa = this.more_from_da |> Seq.map (fun o -> o :> IBclDeviation)