namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IBclDeviantArtProfileFeedItemCollection =
    abstract member Folderid: Guid
    abstract member Name: string
    abstract member Url: string

type DeviantArtProfileFeedItemCollection = {
    folderid: Guid
    name: string
    url: string
} with
    interface IBclDeviantArtProfileFeedItemCollection with
        member this.Folderid = this.folderid
        member this.Name = this.name
        member this.Url = this.url

type IBclDeviantArtProfileFeedItem =
    abstract member Ts: DateTimeOffset
    abstract member Type: string
    abstract member ByUser: IBclDeviantArtUser
    abstract member Status: IBclDeviantArtStatus
    abstract member Deviations: seq<IBclDeviation>
    abstract member Comment: IBclDeviantArtComment
    abstract member CommentParent: IBclDeviantArtComment
    abstract member CommentDeviation: IBclDeviation
    abstract member CommentProfile: IBclDeviantArtProfile
    abstract member CritiqueText: string
    abstract member Collection: IBclDeviantArtProfileFeedItemCollection
    abstract member Formerly: string
    abstract member Poll: IBclDeviantArtFeedItemPoll

type DeviantArtProfileFeedItem = {
    ts: DateTimeOffset
    ``type``: string
    by_user: DeviantArtUser
    status: DeviantArtStatus option
    deviations: Deviation[] option
    comment: DeviantArtComment option
    comment_parent: DeviantArtComment option
    comment_deviation: Deviation option
    comment_profile: DeviantArtProfile option
    critique_text: string option
    collection: DeviantArtProfileFeedItemCollection option
    formerly: string option
    poll: DeviantArtFeedItemPoll option
} with
    interface IBclDeviantArtProfileFeedItem with
        member this.ByUser = this.by_user :> IBclDeviantArtUser
        member this.Collection = this.collection |> Option.map (fun o -> o :> IBclDeviantArtProfileFeedItemCollection) |> Option.toObj
        member this.Comment = this.comment |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.CommentDeviation = this.comment_deviation |> Option.map (fun o -> o :> IBclDeviation) |> Option.toObj
        member this.CommentParent = this.comment_parent |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.CommentProfile = this.comment_profile |> Option.map (fun o -> o :> IBclDeviantArtProfile) |> Option.toObj
        member this.CritiqueText = this.critique_text |> Option.toObj
        member this.Deviations = this.deviations |> Option.map Seq.ofArray |> Option.defaultValue Seq.empty |> Seq.map (fun o -> o :> IBclDeviation)
        member this.Formerly = this.formerly |> Option.toObj
        member this.Poll = this.poll |> Option.map (fun o -> o :> IBclDeviantArtFeedItemPoll) |> Option.toObj
        member this.Status = this.status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj
        member this.Ts = this.ts
        member this.Type = this.``type``