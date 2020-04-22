using System;

namespace DeviantArtFs.Types {
    public class DeviantArtUserDetails {
        public string? sex { get; set; }
        public int? age { get; set; }
        public DateTimeOffset joindate { get; set; }
    }

    public class DeviantArtUserGeo {
        public string country { get; set; } = "";
        public int countryid { get; set; }
        public string timezone { get; set; } = "";
    }

    public class DeviantArtUserProfile {
        public bool user_is_artist { get; set; }
        public string? artist_level { get; set; }
        public string? artist_specialty { get; set; }
        public string real_name { get; set; } = "";
        public string tagline { get; set; } = "";
        public string website { get; set; } = "";
        public string cover_photo { get; set; } = "";
        public Deviation? profile_pic { get; set; }
    }

    public class DeviantArtUserStats {
        public int watchers { get; set; }
        public int friends { get; set; }
    }

    public class DeviantArtUser {
        public Guid userid { get; set; }
        public string username { get; set; } = "";
        public string usericon { get; set; } = "";
        public string type { get; set; } = "";
        public bool? is_watching { get; set; }
        public DeviantArtUserDetails? details { get; set; }
        public DeviantArtUserGeo? geo { get; set; }
        public DeviantArtUserProfile? profile { get; set; }
        public DeviantArtUserStats? stats { get; set; }
    }
}
