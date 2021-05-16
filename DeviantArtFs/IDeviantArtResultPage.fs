namespace DeviantArtFs

/// A page of results from the DeviantArt API.
type IDeviantArtResultPage<'cursor, 'item> =
    /// The current cursor or offset. If Some, used to get the next page; if None, this is the last page.
    abstract member Cursor: 'cursor option
    /// A list of items in this page.
    abstract member Items: 'item seq