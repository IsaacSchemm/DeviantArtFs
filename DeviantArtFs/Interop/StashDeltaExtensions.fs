namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module StashDeltaExtensions =
    [<Extension>]
    let GetItemId (this: StashDeltaEntry) =
        OptUtils.longDefault this.itemid

    [<Extension>]
    let GetStackId (this: StashDeltaEntry) =
        OptUtils.longDefault this.stackid

    [<Extension>]
    let GetPosition (this: StashDeltaEntry) =
        OptUtils.intDefault this.position

    [<Extension>]
    let GetMetadata (this: StashDeltaEntry) =
        OptUtils.recordDefault this.metadata