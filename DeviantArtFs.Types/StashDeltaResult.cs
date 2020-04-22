using System;
using System.Collections.Generic;

namespace DeviantArtFs.Types {
    public class StashDeltaResult : IResultPage<int, StashDeltaEntry> {
        public string cursor { get; set; } = "";
        public bool has_more { get; set; }
        public int? next_offset { get; set; }
        public bool reset { get; set; }
        public StashDeltaEntry[] entries { get; set; } = new StashDeltaEntry[0];

        bool IResultPage<int, StashDeltaEntry>.HasMore => has_more;
        int IResultPage<int, StashDeltaEntry>.Cursor => next_offset ?? 0;
        IEnumerable<StashDeltaEntry> IResultPage<int, StashDeltaEntry>.Items => entries;
    }
}
