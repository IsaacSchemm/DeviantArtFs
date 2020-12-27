namespace DeviantArtFs

/// A DeviantArt response object with status and error information.
type DeviantArtBaseResponse = {
    status: string option
    error: string option
    error_description: string option
}