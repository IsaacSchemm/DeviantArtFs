namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtMessageExtensions =
    [<Extension>]
    let Enumerate (this: DeviantArtMessageSubjectObject) = seq {
        yield! OptUtils.toObjSeq this.profile
        yield! OptUtils.toObjSeq this.deviation
        yield! OptUtils.toObjSeq this.status
        yield! OptUtils.toObjSeq this.comment
        yield! OptUtils.toObjSeq this.collection
        yield! OptUtils.toObjSeq this.gallery
    }

    [<Extension>]
    let GetTimestamp (this: DeviantArtMessage) =
        OptUtils.timeDefault this.ts

    [<Extension>]
    let GetOriginator (this: DeviantArtMessage) =
        OptUtils.recordDefault this.originator

    [<Extension>]
    let GetSubjects (this: DeviantArtMessage) =
        this.subject
        |> Option.map Enumerate
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