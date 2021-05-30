namespace DeviantArtFs.ResponseTypes

/// A DeviantArt response object with status and error information.
type BaseResponse = {
    status: string option
    error: string option
    error_description: string option
}