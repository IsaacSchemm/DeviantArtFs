using System;

namespace DeviantArtFs.Types {
    public class DeviantArtMoreLikeThisPreviewResult {
        public Guid seed { get; set; }
        public DeviantArtUser author { get; set; } = new DeviantArtUser();
        public Deviation[] more_from_artist { get; set; } = new Deviation[0];
        public Deviation[] more_from_da { get; set; } = new Deviation[0];
    }
}
