namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclDeviantArtSubmittedWith =
    abstract member App: string
    abstract member Url: string

type DeviantArtSubmittedWith = {
    app: string
    url: string
} with
    interface IBclDeviantArtSubmittedWith with
        member this.App = this.app
        member this.Url = this.url