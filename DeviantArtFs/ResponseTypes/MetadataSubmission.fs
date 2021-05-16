namespace DeviantArtFs.ResponseTypes

open System

type MetadataSubmission = {
    creation_time: DateTimeOffset
    category: string
    file_size: string option
    resolution: string option
    submitted_with: SubmittedWith
}