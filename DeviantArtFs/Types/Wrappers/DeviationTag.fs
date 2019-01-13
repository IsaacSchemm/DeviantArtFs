namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclDeviationTag =
    abstract member TagName: string
    abstract member Sponsored: bool
    abstract member Sponsor: string

type DeviationTag(original: MetadataResponse.Tag) =
    member __.TagName = original.TagName
    member __.Sponsored = original.Sponsored
    member __.Sponsor = original.Sponsor
    interface IBclDeviationTag with
        member this.TagName = this.TagName
        member this.Sponsored = this.Sponsored
        member this.Sponsor = this.Sponsor |> Option.toObj

