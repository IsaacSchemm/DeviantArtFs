namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclStashSubmission =
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IDeviantArtSubmittedWith

type StashSubmission(original: StashMetadataResponse.Submission) =
    member __.FileSize = original.FileSize
    member __.Resolution = original.Resolution
    member __.SubmittedWith = original.SubmittedWith |> Option.map (fun w -> {
        new IDeviantArtSubmittedWith with
            member __.App = w.App
            member __.Url = w.Url
    })

    interface IBclStashSubmission with
        member this.FileSize = this.FileSize |> Option.toObj
        member this.Resolution = this.Resolution |> Option.toObj
        member this.SubmittedWith = this.SubmittedWith |> Option.toObj