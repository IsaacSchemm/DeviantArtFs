namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclStashSubmission =
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IBclDeviantArtSubmittedWith

type StashSubmission = {
    file_size: string option
    resolution: string option
    submitted_with: DeviantArtSubmittedWith option
} with
    interface IBclStashSubmission with
        member this.FileSize = this.file_size |> Option.toObj
        member this.Resolution = this.resolution |> Option.toObj
        member this.SubmittedWith = this.submitted_with |> Option.map (fun s -> s :> IBclDeviantArtSubmittedWith) |> Option.toObj