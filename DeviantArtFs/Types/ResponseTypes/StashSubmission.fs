namespace DeviantArtFs

type StashSubmission = {
    file_size: string option
    resolution: string option
    submitted_with: DeviantArtSubmittedWith option
} with
    member this.GetSubmittedWith() = OptUtils.toSeq this.submitted_with