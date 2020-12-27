namespace DeviantArtFs

/// A DeviantArt response object with a success flag and (if false) an error description.
type DeviantArtSuccessOrErrorResponse = {
    success: bool
    error_description: string option
}