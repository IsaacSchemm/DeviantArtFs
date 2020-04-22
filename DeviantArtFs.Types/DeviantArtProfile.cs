using System;

namespace DeviantArtFs.Types {
    public class DeviantArtProfileStats {
        public int user_deviations { get; set; }
        public int user_favourites { get; set; }
        public int user_comments { get; set; }
        public int profile_pageviews { get; set; }
        public int profile_comments { get; set; }
    }

    public class DeviantArtProfile {
        public DeviantArtUser user { get; set; } = new DeviantArtUser();
        public bool is_watching { get; set; }
        public string profile_url { get; set; } = "";
        public bool user_is_artist { get; set; }
        public string? artist_level { get; set; }
        public string? artist_specialty { get; set; }
        public string real_name { get; set; } = "";
        public string tagline { get; set; } = "";
        public int countryid { get; set; }
        public string country { get; set; } = "";
        public string website { get; set; } = "";
        public string bio { get; set; } = "";
        public string? cover_photo { get; set; }
        public Deviation? profile_pic { get; set; }
        public DeviantArtStatus? last_status { get; set; }
        public DeviantArtProfileStats stats { get; set; } = new DeviantArtProfileStats();
        public DeviantArtCollectionFolder[]? collections { get; set; }
        public DeviantArtGalleryFolder[]? galleries { get; set; }
    }
}
