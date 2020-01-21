namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtCommentExtensions =
    [<Extension>]
    let GetParentId (this: DeviantArtComment) =
        OptUtils.guidDefault this.parentid

    [<Extension>]
    let GetHidden (this: DeviantArtComment) =
        OptUtils.stringDefault this.hidden

    [<Extension>]
    let GetParent (this: DeviantArtCommentSiblingsContext) =
        OptUtils.recordDefault this.parent

    [<Extension>]
    let GetItems (this: DeviantArtCommentSiblingsContext) = seq {
        yield! OptUtils.toObjSeq this.item_profile
        yield! OptUtils.toObjSeq this.item_deviation
        yield! OptUtils.toObjSeq this.item_status
    }

    [<Extension>]
    let GetNextOffset (this: DeviantArtCommentSiblingsPagedResult) =
        OptUtils.intDefault this.next_offset

    [<Extension>]
    let GetPrevOffset (this: DeviantArtCommentSiblingsPagedResult) =
        OptUtils.intDefault this.prev_offset