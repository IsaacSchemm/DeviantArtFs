using System;

namespace DeviantArtFs.Types {
    public abstract class DeviantArtFolder {
        public Guid folderid { get; set; }
        public string name { get; set; } = "";
        public int? size { get; set; }
        public Deviation[]? deviations { get; set; }
    }

    public class DeviantArtGalleryFolder : DeviantArtFolder {
        public Guid? parent { get; set; }
    }

    public class DeviantArtCollectionFolder : DeviantArtFolder { }
}
