using System;

namespace DeviantArtFs.Types {
    public class DeviantArtBaseResponse {
        public string status { get; set; } = "";
        public string error { get; set; } = "";
        public string error_description { get; set; } = "";
    }
}
