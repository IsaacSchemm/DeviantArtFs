using System;

namespace DeviantArtFs.Types {
    public abstract class DeviationFile {
        public string src { get; set; } = "";
        public int width { get; set; }
        public int height { get; set; }
    }

    public class DeviationPreview : DeviationFile {
        public int filesize { get; set; }
    }

    public class DeviationContent : DeviationPreview {
        public bool transparency { get; set; }
    }

    public class DeviationFlash : DeviationFile { }
}
