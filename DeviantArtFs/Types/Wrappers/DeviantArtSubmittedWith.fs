namespace DeviantArtFs

[<AllowNullLiteral>]
type IDeviantArtSubmittedWith =
    abstract member App: string
    abstract member Url: string

type DeviantArtSubmittedWith = {
    app: string
    url: string
} with
    interface IDeviantArtSubmittedWith with
        member this.App = this.app
        member this.Url = this.url