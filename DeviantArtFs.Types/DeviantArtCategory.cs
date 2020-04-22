using System;

namespace DeviantArtFs.Types {
    public class DeviantArtCategory {
        public string catpath { get; set; } = "";
        public string title { get; set; } = "";
        public bool has_subcategory { get; set; }
        public string parent_catpath { get; set; } = "";
    }

    public class DeviantArtCategoryList {
        public DeviantArtCategory[] categories { get; set; } = new DeviantArtCategory[0];
    }
}
