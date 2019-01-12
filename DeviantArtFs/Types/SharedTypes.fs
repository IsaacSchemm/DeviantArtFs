namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string

[<AllowNullLiteral>]
type IDeviationPreview =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int
    abstract member Transparency: bool

[<AllowNullLiteral>]
type IDeviationContent =
    inherit IDeviationPreview
    abstract member Filesize: int

[<AllowNullLiteral>]
type IDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

[<AllowNullLiteral>]
type IMetadataSubmittedWith =
    abstract member App: string
    abstract member Url: string

[<AllowNullLiteral>]
type IMetadataSubmission =
    abstract member CreationTime: DateTimeOffset
    abstract member Category: string
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IMetadataSubmittedWith

[<AllowNullLiteral>]
type IMetadataStats =
    abstract member Views: int
    abstract member ViewsToday: int
    abstract member Favourites: int
    abstract member Comments: int
    abstract member Downloads: int
    abstract member DownloadsToday: int

type IProfileStats =
    abstract member UserDeviations: int
    abstract member UserFavourites: int
    abstract member UserComments: int
    abstract member ProfilePageviews: int
    abstract member ProfileComments: int