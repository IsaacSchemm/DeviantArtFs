namespace DeviantArtFs.Interop

open System

[<AllowNullLiteral>]
type IStashFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Transparency: bool
