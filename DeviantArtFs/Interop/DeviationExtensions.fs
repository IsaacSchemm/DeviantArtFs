namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviationExtensions =
    [<Extension>]
    let GetSuggester (this: DailyDeviation) =
        OptUtils.recordDefault this.suggester

    [<Extension>]
    let GetPrintId (this: ExistingDeviation) =
        OptUtils.guidDefault this.printid

    [<Extension>]
    let GetPreview (this: ExistingDeviation) =
        OptUtils.recordDefault this.preview

    [<Extension>]
    let GetContent (this: ExistingDeviation) =
        OptUtils.recordDefault this.content

    [<Extension>]
    let GetVideos (this: ExistingDeviation) =
        OptUtils.listDefault this.videos

    [<Extension>]
    let GetFlash (this: ExistingDeviation) =
        OptUtils.recordDefault this.flash

    [<Extension>]
    let GetDailyDeviation (this: ExistingDeviation) =
        OptUtils.recordDefault this.daily_deviation

    [<Extension>]
    let GetExcerpt (this: ExistingDeviation) =
        OptUtils.stringDefault this.excerpt

    [<Extension>]
    let GetDownloadFilesize (this: ExistingDeviation) =
        OptUtils.intDefault this.download_filesize

    [<Extension>]
    let GetHtml (this: DeviationTextContent) =
        OptUtils.stringDefault this.html

    [<Extension>]
    let GetCss (this: DeviationTextContent) =
        OptUtils.stringDefault this.css

    [<Extension>]
    let GetCssFonts (this: DeviationTextContent) =
        OptUtils.listDefault this.css_fonts

    [<Extension>]
    let GetSponsor (this: DeviationTag) =
        OptUtils.stringDefault this.sponsor