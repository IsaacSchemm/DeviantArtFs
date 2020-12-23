namespace DeviantArtFs

/// An object that specifies which optional fields to return when fetchinig
/// deviation or Sta.sh submission metadata.
type DeviantArtExtParams = {
    /// Whether to include extended submission information, such as category and file size.
    ExtSubmission: bool
    /// Whether to include EXIF information from the camera.
    ExtCamera: bool
    /// Whether to include extended statistics, such as the number of views and comments.
    ExtStats: bool
} with
    /// An object whose parameters tell the API to return none of the optional fields.
    static member None = {
        ExtSubmission = false
        ExtCamera = false
        ExtStats = false
    }
    /// An object whose parameters tell the API to return all of the optional fields.
    static member All = {
        ExtSubmission = true
        ExtCamera = true
        ExtStats = true
    }