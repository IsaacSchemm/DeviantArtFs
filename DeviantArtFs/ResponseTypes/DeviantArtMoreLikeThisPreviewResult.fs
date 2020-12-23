namespace DeviantArtFs

open System
open FSharp.Json

type FeaturedInCollection = {
    collection: DeviantArtGallection
    deviations: Deviation list
}

type DeviantArtMoreLikeThisPreviewResult = {
    seed: Guid
    author: DeviantArtUser
    more_from_artist: Deviation list
    more_from_da: Deviation list
    featured_in_collections: FeaturedInCollection list option
} with
    static member Parse json = Json.deserialize<DeviantArtMoreLikeThisPreviewResult> json
    member this.FeaturedInCollections = this.featured_in_collections |> Option.defaultValue List.empty