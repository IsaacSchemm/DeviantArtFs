namespace DeviantArtFs

open System

type DeviationMetadataSubmission = {
    creation_time: DateTimeOffset
    category: string
    file_size: string option
    resolution: string option
    submitted_with: DeviantArtSubmittedWith
} with
    member this.GetFileSize() = OptUtils.stringDefault this.file_size
    member this.GetResolution() = OptUtils.stringDefault this.resolution