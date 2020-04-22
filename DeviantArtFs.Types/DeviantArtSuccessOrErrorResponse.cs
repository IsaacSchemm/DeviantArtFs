using System;

namespace DeviantArtFs.Types {
    public class DeviantArtSuccessOrErrorResponse {
        public bool success { get; set; }
        public string? error_description { get; set; }
    }
}
