namespace DeviantArtFs.Interop

open System
open DeviantArtFs

[<AllowNullLiteral>]
type IDeviantArtFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Filesize: Nullable<int>

[<AllowNullLiteral>]
type IStashFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Transparency: bool
