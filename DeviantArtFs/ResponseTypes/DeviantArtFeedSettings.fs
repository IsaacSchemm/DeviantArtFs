namespace DeviantArtFs

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
}