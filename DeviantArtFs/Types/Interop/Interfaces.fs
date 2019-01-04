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

type IDeviantArtCollection =
    abstract member Folderid: Guid
    abstract member Name: string

type IDeviantArtFolder =
    abstract member Folderid: Guid
    abstract member Parent: Nullable<Guid>
    abstract member Name: string
    abstract member Size: Nullable<int>

[<AllowNullLiteral>]
type IDeviantArtFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Filesize: Nullable<int>

type IDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member Metadata: string
    abstract member Position: Nullable<int>

type IDeltaResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member NextOffset: System.Nullable<int>
    abstract member Reset: bool
    abstract member Entries: seq<IDeltaEntry>

type IMoveResult =
    abstract member Target: string
    abstract member Changes: seq<string>
