namespace DeviantArtFs

/// A DeviantArt response that contains only a "results" field.
type DeviantArtListOnlyResponse<'a> = {
    results: 'a list
}