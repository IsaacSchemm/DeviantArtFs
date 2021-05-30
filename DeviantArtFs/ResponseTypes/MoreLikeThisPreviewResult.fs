namespace DeviantArtFs.ResponseTypes

open System

type FeaturedInCollection = {
    collection: Gallection
    deviations: Deviation list
}

type MoreLikeThisPreviewResult = {
    seed: Guid
    author: User
    more_from_artist: Deviation list
    more_from_da: Deviation list
    featured_in_collections: FeaturedInCollection list option
} with
    member this.FeaturedInCollections = this.featured_in_collections |> Option.defaultValue List.empty