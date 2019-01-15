namespace DeviantArtFs

open System

type IMoreLikeThisPreviewResult<'a, 'b> =
    abstract member Seed: Guid
    abstract member Author: 'a
    abstract member MoreFromArtist: seq<'b>
    abstract member MoreFromDa: seq<'b>

type IStashPublishResult =
    abstract member Url: string
    abstract member DeviationId: Guid

type IStashPublishUserdataResult =
    abstract member Features: seq<string>
    abstract member Agreements: seq<string>

type IStashSpaceResult =
    abstract member AvailableSpace: int64
    abstract member TotalSpace: int64

type IDeviantArtCategory =
    abstract member Catpath: string
    abstract member Title: string
    abstract member HasSubcategory: bool
    abstract member ParentCatpath: string