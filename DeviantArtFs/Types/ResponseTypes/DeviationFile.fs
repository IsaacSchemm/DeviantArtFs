namespace DeviantArtFs

open FSharp.Json

type DeviationPreview = {
    src: string
    height: int
    width: int
    transparency: bool
}

type DeviationDownload = {
    src: string
    height: int
    width: int
    filesize: int
} with
    static member Parse json = Json.deserialize<DeviationDownload> json

type DeviationContent = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
}