using System;

namespace DeviantArtFs.Types {
    public class StashMoveResult {
        public StashMetadata target { get; set; } = new StashMetadata();
        public StashMetadata[] changes { get; set; } = new StashMetadata[0];
    }
}
