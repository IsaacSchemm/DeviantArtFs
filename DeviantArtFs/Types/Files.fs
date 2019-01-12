namespace DeviantArtFs

[<AllowNullLiteral>]
type IDeviationPreview =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Transparency: bool

[<AllowNullLiteral>]
type IDeviationContent =
    inherit IDeviationPreview
    abstract member Filesize: int