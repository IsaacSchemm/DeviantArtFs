namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtMessageExtensions =
    [<Extension>]
    let GetTimestamp (this: DeviantArtMessage) =
        OptUtils.timeDefault this.ts

    [<Extension>]
    let GetOriginator (this: DeviantArtMessage) =
        OptUtils.recordDefault this.originator

    [<Extension>]
    let GetSubjects (this: DeviantArtMessage) =
        this.subject
        |> Option.map DeviantArtExtensions.GetMessageSubject
        |> Option.defaultValue Seq.empty

    [<Extension>]
    let GetStackId (this: DeviantArtMessage) =
        OptUtils.stringDefault this.stackid

    [<Extension>]
    let GetStackCount (this: DeviantArtMessage) =
        OptUtils.intDefault this.stack_count


    [<Extension>]
    let GetHtml (this: DeviantArtMessage) =
        OptUtils.recordDefault this.html

    [<Extension>]
    let GetProfile (this: DeviantArtMessage) =
        OptUtils.recordDefault this.profile

    [<Extension>]
    let GetDeviation (this: DeviantArtMessage) =
        OptUtils.recordDefault this.deviation

    [<Extension>]
    let GetStatus (this: DeviantArtMessage) =
        OptUtils.recordDefault this.status

    [<Extension>]
    let GetComment (this: DeviantArtMessage) =
        OptUtils.recordDefault this.comment

    [<Extension>]
    let GetCollection (this: DeviantArtMessage) =
        OptUtils.recordDefault this.collection