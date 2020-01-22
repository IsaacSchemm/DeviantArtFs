namespace DeviantArtFs

[<AllowNullLiteral>]
type IDeviantArtDeletable =
    abstract member IsDeleted: bool