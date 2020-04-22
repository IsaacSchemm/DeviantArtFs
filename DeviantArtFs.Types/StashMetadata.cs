using System;
using System.Collections.Generic;

namespace DeviantArtFs.Types {
    public class StashSubmission {
        public string? file_size { get; set; }
        public string? resolution { get; set; }
        public DeviantArtSubmittedWith? submitted_with { get; set; }
    }

    public class StashStats {
        public int? views { get; set; }
        public int? views_today { get; set; }
        public int? downloads { get; set; }
        public int? downloads_today { get; set; }
    }

    public class StashMetadata {
        public string title { get; set; } = "";
        public string? path { get; set; }
        public int? size { get; set; }
        public string? description { get; set; }
        public long? parentid { get; set; }
        public DeviationPreview thumb { get; set; } = new DeviationPreview();
        public string? artist_comments { get; set; }
        public string? original_url { get; set; }
        public string? category { get; set; }
        public DateTimeOffset? creation_time { get; set; }
        public DeviationPreview[]? files { get; set; }
        public StashSubmission? submission { get; set; }
        public StashStats? stats { get; set; }
        public Dictionary<string, string>? camera { get; set; }
        public long? stackid { get; set; }
        public long? itemid { get; set; }
        public string[]? tags { get; set; }
    }
}
