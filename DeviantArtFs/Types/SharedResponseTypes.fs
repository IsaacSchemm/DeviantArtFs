namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string

[<AllowNullLiteral>]
type IDeviationFile =
    abstract member Src: string
    abstract member Height: int
    abstract member Width: int

[<AllowNullLiteral>]
type IDeviationPreview =
    inherit IDeviationFile
    abstract member Transparency: bool

[<AllowNullLiteral>]
type IDeviationDownload =
    inherit IDeviationFile
    abstract member Filesize: int

[<AllowNullLiteral>]
type IDeviationContent =
    inherit IDeviationPreview
    inherit IDeviationDownload

type IMoreLikeThisPreviewResult<'a> =
    abstract member Seed: Guid
    abstract member Author: IDeviantArtUser
    abstract member MoreFromArtist: seq<'a>
    abstract member MoreFromDa: seq<'a>

[<AllowNullLiteral>]
type IDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

[<AllowNullLiteral>]
type IDeviantArtSubmittedWith =
    abstract member App: string
    abstract member Url: string

[<AllowNullLiteral>]
type IDeviationMetadataSubmission =
    abstract member CreationTime: DateTimeOffset
    abstract member Category: string
    abstract member FileSize: string
    abstract member Resolution: string
    abstract member SubmittedWith: IDeviantArtSubmittedWith

[<AllowNullLiteral>]
type IDeviationMetadataStats =
    abstract member Views: int
    abstract member ViewsToday: int
    abstract member Favourites: int
    abstract member Comments: int
    abstract member Downloads: int
    abstract member DownloadsToday: int

type IWhoFavedUser =
    abstract member User: IDeviantArtUser
    abstract member Time: int64

type IStashPublishResult =
    abstract member Url: string
    abstract member DeviationId: Guid

type IStashPublishUserdataResult =
    abstract member Features: seq<string>
    abstract member Agreements: seq<string>

type IStashSpaceResult =
    abstract member AvailableSpace: int64
    abstract member TotalSpace: int64

type IDeviantArtProfileStats =
    abstract member UserDeviations: int
    abstract member UserFavourites: int
    abstract member UserComments: int
    abstract member ProfilePageviews: int
    abstract member ProfileComments: int

type IDeviantArtCategory =
    abstract member Catpath: string
    abstract member Title: string
    abstract member HasSubcategory: bool
    abstract member ParentCatpath: string