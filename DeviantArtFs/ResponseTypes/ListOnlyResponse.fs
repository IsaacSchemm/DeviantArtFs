namespace DeviantArtFs.ResponseTypes

/// A DeviantArt response that contains only a "results" field.
type ListOnlyResponse<'a> = {
    results: 'a list
}