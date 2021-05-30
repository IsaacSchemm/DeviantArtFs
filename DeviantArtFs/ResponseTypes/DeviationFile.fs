namespace DeviantArtFs.ResponseTypes

type Preview = {
    src: string
    height: int
    width: int
    transparency: bool
}

type Download = {
    src: string
    height: int
    width: int
    filesize: int
}

type Content = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
}