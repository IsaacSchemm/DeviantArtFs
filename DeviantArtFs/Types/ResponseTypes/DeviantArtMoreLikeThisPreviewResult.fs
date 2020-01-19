namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtMoreLikeThisPreviewResult = {
    seed: Guid
    author: DeviantArtUser
    more_from_artist: Deviation list
    more_from_da: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtMoreLikeThisPreviewResult> json
    member this.GetMoreFromArtist() = OptUtils.listDefault this.more_from_artist
    member this.GetMoreFromDA() = OptUtils.listDefault this.more_from_da