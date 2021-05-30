namespace DeviantArtFs.ResponseTypes

/// A DeviantArt response object with a success flag and (if false) an error description.
type SuccessOrErrorResponse = {
    success: bool
    error_description: string option
}