namespace DeviantArtFs

[<AllowNullLiteral>]
type IDeviationFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int

[<AllowNullLiteral>]
type IDeviationPreview =
    inherit IDeviationFile
    abstract member Transparency: bool

type DeviationPreview = {
    src: string
    height: int
    width: int
    transparency: bool
} with
    interface IDeviationPreview with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width
        member this.Transparency = this.transparency

[<AllowNullLiteral>]
type IDeviationDownload =
    inherit IDeviationFile
    abstract member Filesize: int

type DeviationDownload = {
    src: string
    height: int
    width: int
    filesize: int
} with
    interface IDeviationDownload with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width
        member this.Filesize = this.filesize

[<AllowNullLiteral>]
type IDeviationContent =
    inherit IDeviationPreview
    inherit IDeviationDownload

type DeviationContent = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
} with
    interface IDeviationContent with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width
        member this.Transparency = this.transparency
        member this.Filesize = this.filesize