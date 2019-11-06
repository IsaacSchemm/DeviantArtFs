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

/// An object that specifies which optional fields to return when fetchinig
/// deviation or Sta.sh submission metadata.
type DeviantArtExtParams() =
    /// Whether to include extended submission information, such as category and file size.
    member val ExtSubmission = false with get, set
    /// Whether to include EXIF information from the camera.
    member val ExtCamera = false with get, set
    /// Whether to include extended statistics, such as the number of views and comments.
    member val ExtStats = false with get, set

    /// An object whose parameters tell the API to return none of the optional fields.
    static member None = {
        new IDeviantArtExtParams with
            member __.ExtSubmission = false
            member __.ExtCamera = false
            member __.ExtStats = false
        }
    /// An object whose parameters tell the API to return all of the optional fields.
    static member All = {
        new IDeviantArtExtParams with
            member __.ExtSubmission = true
            member __.ExtCamera = true
            member __.ExtStats = true
        }
    interface IDeviantArtExtParams with
        member this.ExtSubmission = this.ExtSubmission
        member this.ExtCamera = this.ExtCamera
        member this.ExtStats = this.ExtStats