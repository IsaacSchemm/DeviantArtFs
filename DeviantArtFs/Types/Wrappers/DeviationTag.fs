namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclDeviationTag =
    abstract member TagName: string
    abstract member Sponsored: bool
    abstract member Sponsor: string

type DeviationTag = {
    tag_name: string
    sponsored: bool
    sponsor: string option
} with
    interface IBclDeviationTag with
        member this.TagName = this.tag_name
        member this.Sponsored = this.sponsored
        member this.Sponsor = this.sponsor |> Option.toObj