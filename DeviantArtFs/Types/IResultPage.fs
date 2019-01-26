namespace DeviantArtFs

type internal IResultPage<'cursor, 'item> =
    abstract member HasMore: bool
    abstract member Cursor: 'cursor
    abstract member Items: seq<'item>