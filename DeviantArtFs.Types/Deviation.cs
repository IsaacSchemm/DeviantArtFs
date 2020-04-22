using System;

namespace DeviantArtFs.Types {
    public class DeviationStats {
        public int comments { get; set; }
        public int favourites { get; set; }
    }

    public class DeviationVideo {
        public string src { get; set; } = "";
        public string quality { get; set; } = "";
        public int filesize { get; set; }
        public int duration { get; set; }
    }

    public class DailyDeviation {
        public string body { get; set; } = "";
        public DateTimeOffset time { get; set; }
        public DeviantArtUser giver { get; set; } = new DeviantArtUser();
        public DeviantArtUser? suggester { get; set; }
    }

    public class Deviation {
        public Guid deviationid { get; set; }
        public Guid? printid { get; set; }
        public string? url { get; set; }
        public string? title { get; set; }
        public string? category { get; set; }
        public string? category_path { get; set; }
        public bool? is_favourited { get; set; }
        public bool is_deleted { get; set; }
        public DeviantArtUser? author { get; set; }
        public DeviationStats? stats { get; set; }
        public DateTimeOffset? published_time { get; set; }
        public bool? allows_comments { get; set; }
        public DeviationPreview? preview { get; set; }
        public DeviationContent? content { get; set; }
        public DeviationPreview? thumbs { get; set; }
        public DeviationVideo? videos { get; set; }
        public DeviationFlash? flash { get; set; }
        public DailyDeviation? daily_deviation { get; set; }
        public string? excerpt { get; set; }
        public bool? is_mature { get; set; }
        public bool? is_downloadable { get; set; }
        public int? download_filesize { get; set; }
    }
}
