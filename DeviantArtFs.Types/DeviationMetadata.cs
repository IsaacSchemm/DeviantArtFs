using System;
using System.Collections.Generic;

namespace DeviantArtFs.Types {
    public class DeviationMetadataStats {
        public int views { get; set; }
        public int views_today { get; set; }
        public int favourites { get; set; }
        public int comments { get; set; }
        public int downloads { get; set; }
        public int downloads_today { get; set; }
    }

    public class DeviationMetadataSubmission {
        public DateTimeOffset creation_time { get; set; }
        public string category { get; set; } = "";
        public string? file_size { get; set; }
        public string? resolution { get; set; }
        public DeviantArtSubmittedWith submitted_with { get; set; } = new DeviantArtSubmittedWith();
    }

    public class DeviationMetadata {
        public Guid deviationid { get; set; }
        public Guid? printid { get; set; }
        public DeviantArtUser author { get; set; } = new DeviantArtUser();
        public bool is_watching { get; set; }
        public string title { get; set; } = "";
        public string description { get; set; } = "";
        public string license { get; set; } = "";
        public bool allows_comments { get; set; }
        public DeviationTag[] tags { get; set; } = new DeviationTag[0];
        public bool is_favourited { get; set; }
        public bool is_mature { get; set; }
        public DeviationMetadataSubmission? submission { get; set; }
        public DeviationMetadataStats? stats { get; set; }
        public Dictionary<string, string>? camera { get; set; }
        public DeviantArtCollectionFolder[]? collections { get; set; }
    }

    public class DeviationMetadataResponse {
        public DeviationMetadata[] metadata { get; set; } = new DeviationMetadata[0];
    }
}
