namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviationMetadataExtensions =
    [<Extension>]
    let GetFileSize (this: DeviationMetadataSubmission) =
        OptUtils.stringDefault this.file_size

    [<Extension>]
    let GetResolution (this: DeviationMetadataSubmission) =
        OptUtils.stringDefault this.resolution

    [<Extension>]
    let GetPrintId (this: DeviationMetadata) =
        OptUtils.guidDefault this.printid

    [<Extension>]
    let GetSubmission (this: DeviationMetadata) =
        OptUtils.recordDefault this.submission

    [<Extension>]
    let GetStats (this: DeviationMetadata) =
        OptUtils.recordDefault this.stats

    [<Extension>]
    let GetCamera (this: DeviationMetadata) =
        OptUtils.mapDefault this.camera

    [<Extension>]
    let GetCollections (this: DeviationMetadata) =
        OptUtils.listDefault this.collections