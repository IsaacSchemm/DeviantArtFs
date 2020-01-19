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
}

type DeviationContent = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
}