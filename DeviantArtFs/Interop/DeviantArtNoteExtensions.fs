namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtNoteExtensions =
    [<Extension>]
    let GetRawBody (this: DeviantArtNote) =
        OptUtils.stringDefault this.raw_body

    [<Extension>]
    let GetParentId (this: DeviantArtNotesFolder) =
        OptUtils.guidDefault this.parentid