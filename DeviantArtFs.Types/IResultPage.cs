using System.Collections.Generic;

namespace DeviantArtFs.Types {
    /// <summary>
    /// A page of results from the DeviantArt API.
    /// This interface is used to handle paging in ToAsyncSeq and ToArrayAsync methods.
    /// </summary>
    public interface IResultPage<TCursor, TItem> {
        /// <summary>
        /// Whether there are more results after this page.
        /// </summary>
        bool HasMore { get; }

        /// <summary>
        /// The current cursor or offset. Used to get the next page.
        /// </summary>
        TCursor Cursor { get; }

        /// <summary>
        /// A list of items in this page.
        /// </summary>
        IEnumerable<TItem> Items { get; }
    }
}
