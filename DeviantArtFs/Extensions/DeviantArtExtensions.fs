namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtExtensions =
    [<Extension>]
    let GetEmbeddedDeviations (seq: DeviantArtStatusItem seq) =
        seq |> Seq.choose (fun i -> i.deviation)

    [<Extension>]
    let GetEmbeddedStatuses (seq: DeviantArtStatusItem seq) =
        seq |> Seq.choose (fun i -> i.status)

    [<Extension>]
    let WhereDeviationNotDeleted (s: Deviation seq) =
        seq {
        for d in s do
            if not (isNull (d :> obj)) then
                match d.ToUnion() with
                | DeviationUnion.Deleted -> ()
                | DeviationUnion.Existing e -> yield e
    }

    [<Extension>]
    let WhereStatusNotDeleted (s: DeviantArtStatus seq) = seq {
        for d in s do
            if not (isNull (d :> obj)) then
                match d.ToUnion() with
                | Deleted -> ()
                | Existing e -> yield e
    }

    [<Extension>]
    let GetCommentContextItem (this: DeviantArtCommentSiblingsContext) = seq {
        yield! OptUtils.toObjSeq this.item_profile
        yield! OptUtils.toObjSeq this.item_deviation
        yield! OptUtils.toObjSeq this.item_status
    }

    [<Extension>]
    let GetMessageSubject (this: DeviantArtMessageSubjectObject) = seq {
        yield! OptUtils.toObjSeq this.profile
        yield! OptUtils.toObjSeq this.deviation
        yield! OptUtils.toObjSeq this.status
        yield! OptUtils.toObjSeq this.comment
        yield! OptUtils.toObjSeq this.collection
        yield! OptUtils.toObjSeq this.gallery
    }