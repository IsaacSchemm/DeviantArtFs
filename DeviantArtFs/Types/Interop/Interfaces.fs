namespace DeviantArtFs.Interop

open System
open DeviantArtFs

type ICategory =
    abstract member Catpath: string
    abstract member Title: string
    abstract member HasSubcategory: bool
    abstract member ParentCatpath: string

type IContentResult =
    abstract member Html: string
    abstract member Css: string
    abstract member CssFonts: seq<string>

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
