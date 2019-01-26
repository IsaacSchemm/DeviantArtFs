namespace DeviantArtFs

type IDeviantArtExtParams =
    abstract member ExtSubmission: bool
    abstract member ExtCamera: bool
    abstract member ExtStats: bool

type DeviantArtExtParams() =
    member val ExtSubmission = false with get, set
    member val ExtCamera = false with get, set
    member val ExtStats = false with get, set
    static member None = {
        new IDeviantArtExtParams with
            member __.ExtSubmission = false
            member __.ExtCamera = false
            member __.ExtStats = false
        }
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