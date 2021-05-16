namespace DeviantArtFs.ResponseTypes

type StashSubmission = {
    file_size: string option
    resolution: string option
    submitted_with: SubmittedWith option
}