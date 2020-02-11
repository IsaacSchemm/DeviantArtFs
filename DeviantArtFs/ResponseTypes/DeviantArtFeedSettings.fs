namespace DeviantArtFs

open FSharp.Json

type DeviantArtFeedInclude = {
    statuses: bool
    deviations: bool
    journals: bool
    group_deviations: bool
    collections: bool
    misc: bool
}

type DeviantArtFeedSettings = {
    ``include``: DeviantArtFeedInclude
} with
    static member Parse (json: string) = Json.deserialize<DeviantArtFeedSettings> json