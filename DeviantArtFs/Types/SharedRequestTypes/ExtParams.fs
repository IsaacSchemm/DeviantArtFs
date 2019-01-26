namespace DeviantArtFs

type IExtParams =
    abstract member ExtSubmission: bool
    abstract member ExtCamera: bool
    abstract member ExtStats: bool

type ExtParams() =
    member val ExtSubmission = false with get, set
    member val ExtCamera = false with get, set
    member val ExtStats = false with get, set
    static member None = {
        new IExtParams with
            member __.ExtSubmission = false
            member __.ExtCamera = false
            member __.ExtStats = false
        }
    static member All = {
        new IExtParams with
            member __.ExtSubmission = true
            member __.ExtCamera = true
            member __.ExtStats = true
        }
    interface IExtParams with
        member this.ExtSubmission = this.ExtSubmission
        member this.ExtCamera = this.ExtCamera
        member this.ExtStats = this.ExtStats