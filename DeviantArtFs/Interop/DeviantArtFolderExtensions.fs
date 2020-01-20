namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtFolderExtensions =
    [<Extension>]
    let GetParent (this: DeviantArtFolder) =
        OptUtils.guidDefault this.parent

    [<Extension>]
    let GetSize (this: DeviantArtFolder) =
        OptUtils.intDefault this.size

    [<Extension>]
    let GetDeviations (this: DeviantArtFolder) =
        OptUtils.listDefault this.deviations