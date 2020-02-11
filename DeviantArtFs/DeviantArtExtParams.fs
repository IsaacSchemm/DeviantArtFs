namespace DeviantArtFs

/// An object that specifies which optional fields to return when fetchinig
/// deviation or Sta.sh submission metadata.
type IDeviantArtExtParams =
    /// Whether to include extended submission information, such as category and file size.
    abstract member ExtSubmission: bool
    /// Whether to include EXIF information from the camera.
    abstract member ExtCamera: bool
    /// Whether to include extended statistics, such as the number of views and comments.
    abstract member ExtStats: bool

/// Presets for the IDeviantArtExtParams interface.
module DeviantArtExtParams =
    /// An object whose parameters tell the API to return none of the optional fields.
    let None = {
        new IDeviantArtExtParams with
            member __.ExtSubmission = false
            member __.ExtCamera = false
            member __.ExtStats = false
        }

    /// An object whose parameters tell the API to return all of the optional fields.
    let All = {
        new IDeviantArtExtParams with
            member __.ExtSubmission = true
            member __.ExtCamera = true
            member __.ExtStats = true
        }