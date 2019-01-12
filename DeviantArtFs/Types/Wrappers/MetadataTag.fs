namespace DeviantArtFs

[<AllowNullLiteral>]
type IMetadataTag =
    abstract member TagName: string
    abstract member Sponsored: bool
    abstract member Sponsor: string

type MetadataTag(original: MetadataResponse.Tag) =
    member __.TagName = original.TagName
    member __.Sponsored = original.Sponsored
    member __.Sponsor = original.Sponsor
    interface IMetadataTag with
        member this.TagName = this.TagName
        member this.Sponsored = this.Sponsored
        member this.Sponsor = this.Sponsor |> Option.toObj

