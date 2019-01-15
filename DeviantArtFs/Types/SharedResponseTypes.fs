namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string

type IMoreLikeThisPreviewResult<'a> =
    abstract member Seed: Guid
    abstract member Author: IDeviantArtUser
    abstract member MoreFromArtist: seq<'a>
    abstract member MoreFromDa: seq<'a>

type IWhoFavedUser =
    abstract member User: IDeviantArtUser
    abstract member Time: DateTimeOffset

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