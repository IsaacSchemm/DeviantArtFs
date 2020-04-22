using System;

namespace DeviantArtFs.Types {
    public class DeviantArtWatchInfo {
        public bool friend { get; set; }
        public bool deviations { get; set; }
        public bool journals { get; set; }
        public bool forum_threads { get; set; }
        public bool critiques { get; set; }
        public bool scraps { get; set; }
        public bool activity { get; set; }
        public bool collections { get; set; }
    }

    public abstract class DeviantArtRelationshipRecord {
        public DeviantArtUser user { get; set; } = new DeviantArtUser();
        public bool is_watching { get; set; }
        public DeviantArtWatchInfo watch { get; set; } = new DeviantArtWatchInfo();
    }

    public class DeviantArtFriendRecord : DeviantArtRelationshipRecord {
        public bool watches_you { get; set; }
    }

    public class DeviantArtWatcherRecord : DeviantArtRelationshipRecord {
        public DateTimeOffset? lastvisit { get; set; }
    }
}
