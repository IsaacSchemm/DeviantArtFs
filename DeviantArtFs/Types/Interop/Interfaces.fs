namespace DeviantArtFs.Interop

type IDeviantArtCollection =
    abstract member Folderid: Guid
    abstract member Name: string

type IDeviantArtFolder =
    abstract member Folderid: Guid
    abstract member Parent: Nullable<Guid>
    abstract member Name: string
    abstract member Size: Nullable<int>

type IDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member Metadata: string
    abstract member Position: Nullable<int>