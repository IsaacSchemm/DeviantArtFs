namespace DeviantArtFs

open System

type DeviationMetadataSubmission = {
    creation_time: DateTimeOffset
    category: string
    file_size: string option
    resolution: string option
    submitted_with: DeviantArtSubmittedWith
}