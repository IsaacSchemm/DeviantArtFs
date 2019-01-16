namespace DeviantArtFs

open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviationFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int

[<AllowNullLiteral>]
type IBclDeviationPreview =
    inherit IBclDeviationFile
    abstract member Transparency: bool

type DeviationPreview = {
    src: string
    height: int
    width: int
    transparency: bool
} with
    interface IBclDeviationPreview with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width
        member this.Transparency = this.transparency

[<AllowNullLiteral>]
type IBclDeviationDownload =
    inherit IBclDeviationFile
    abstract member Filesize: int

type DeviationDownload = {
    src: string
    height: int
    width: int
    filesize: int
} with
    static member Parse json = Json.deserialize<DeviationDownload> json
    interface IBclDeviationDownload with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width
        member this.Filesize = this.filesize

[<AllowNullLiteral>]
type IBclDeviationContent =
    inherit IBclDeviationPreview
    inherit IBclDeviationDownload

type DeviationContent = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
} with
    interface IBclDeviationContent with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width
        member this.Transparency = this.transparency
        member this.Filesize = this.filesize