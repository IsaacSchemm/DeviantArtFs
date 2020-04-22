using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviantArtFs.Types {
    public class DeviantArtStatusItem {
        public string type { get; set; } = "";
        public DeviantArtStatus? status { get; set; }
        public Deviation? deviation { get; set; }
    }

    public class DeviantArtStatus {
        public Guid? statusid { get; set; }
        public string? body { get; set; }
        public DateTimeOffset? ts { get; set; }
        public string? url { get; set; }
        public int? comments_count { get; set; }
        public bool? is_share { get; set; }
        public bool is_deleted { get; set; }
        public DeviantArtUser? author { get; set; }
        public DeviantArtStatusItem[]? items { get; set; }

        public IEnumerable<Deviation> GetEmbeddedDeviations() {
            if (items == null) yield break;
            foreach (var i in items) {
                if (i.deviation != null) yield return i.deviation;
            }
        }

        public IEnumerable<DeviantArtStatus> GetEmbeddedStatuses() {
            if (items == null) yield break;
            foreach (var i in items) {
                if (i.status != null) yield return i.status;
            }
        }
    }
}
