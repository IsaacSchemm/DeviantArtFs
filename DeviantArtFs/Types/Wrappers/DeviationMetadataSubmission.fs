namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IBclDeviationMetadataSubmission =
    abstract member CreationTime: DateTimeOffset
    abstract member Category: string
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IBclDeviantArtSubmittedWith

type DeviationMetadataSubmission = {
    creation_time: DateTimeOffset
    category: string
    file_size: string option
    resolution: string option
    submitted_with: DeviantArtSubmittedWith
} with
    interface IBclDeviationMetadataSubmission with
        member this.Category = this.category
        member this.CreationTime = this.creation_time
        member this.FileSize = this.file_size |> Option.toObj
        member this.Resolution = this.resolution |> Option.toObj
        member this.SubmittedWith = this.submitted_with :> IBclDeviantArtSubmittedWith