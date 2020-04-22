using System;

namespace DeviantArtFs.Types {
    public class DeviantArtListOnlyResponse<T> {
        public T[] results { get; set; }
    }
}
