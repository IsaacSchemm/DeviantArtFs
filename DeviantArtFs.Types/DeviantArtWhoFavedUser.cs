using System;

namespace DeviantArtFs.Types {
    public class DeviantArtWhoFavedUser {
        public DeviantArtUser user { get; set; } = new DeviantArtUser();
        public DateTimeOffset time { get; set; }
    }
}
