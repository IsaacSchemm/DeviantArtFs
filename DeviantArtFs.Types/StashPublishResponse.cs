using System;

namespace DeviantArtFs.Types {
    public class StashPublishResponse {
        public string status { get; set; } = "";
        public string url { get; set; } = "";
        public Guid deviationid { get; set; }
    }
}
