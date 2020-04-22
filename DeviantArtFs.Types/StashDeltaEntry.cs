using System;

namespace DeviantArtFs.Types {
    public class StashDeltaEntry {
        public long? itemid { get; set; }
        public long? stackid { get; set; }
        public StashMetadata? metadata { get; set; }
        public int? position { get; set; }
    }
}
