namespace DeviantArtFs

open FSharp.Json

type IDeviationFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int

type DeviationPreview = {
    src: string
    height: int
    width: int
    transparency: bool
} with
    interface IDeviationFile with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width

type DeviationDownload = {
    src: string
    height: int
    width: int
    filesize: int
} with
    static member Parse json = Json.deserialize<DeviationDownload> json
    interface IDeviationFile with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width

type DeviationContent = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
} with
    interface IDeviationFile with
        member this.Src = this.src
        member this.Height = this.height
        member this.Width = this.width