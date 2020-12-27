﻿namespace DeviantArtFs

open System

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
    member this.FeaturedInCollections = this.featured_in_collections |> Option.defaultValue List.empty