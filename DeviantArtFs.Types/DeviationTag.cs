using System;

namespace DeviantArtFs.Types {
    public class DeviationTag {
        public string tag_name { get; set; } = "";
        public bool sponsored { get; set; }
        public string? sponsor { get; set; }
    }
}
