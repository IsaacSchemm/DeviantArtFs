namespace DeviantArtFs

/// A page of results from the DeviantArt API.
type internal IResultPage<'cursor, 'item> =
    /// Whether there are more results after this page.
    abstract member HasMore: bool
    /// The current cursor or offset. Used to get the next page.
    abstract member Cursor: 'cursor
    /// A list of items in this page.
    abstract member Items: seq<'item>