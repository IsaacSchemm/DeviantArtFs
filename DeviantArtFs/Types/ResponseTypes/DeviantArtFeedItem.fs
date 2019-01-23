namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IBclDeviantArtFeedItemCollection =
    abstract member Folderid: Guid
    abstract member Name: string
    abstract member Url: string
    abstract member Size: int

type DeviantArtFeedItemCollection = {
    folderid: Guid
    name: string
    url: string
    size: int
} with
    interface IBclDeviantArtFeedItemCollection with
        member this.Folderid = this.folderid
        member this.Name = this.name
        member this.Size = this.size
        member this.Url = this.url

type IBclDeviantArtFeedItemPollAnswer =
    abstract member Answer: string
    abstract member Votes: int

type DeviantArtFeedItemPollAnswer = {
    answer: string
    votes: int
} with
    interface IBclDeviantArtFeedItemPollAnswer with
        member this.Answer = this.answer
        member this.Votes = this.votes
        
[<AllowNullLiteral>]
type IBclDeviantArtFeedItemPoll =
    abstract member Question: string
    abstract member TotalVotes: int
    abstract member Answers: seq<IBclDeviantArtFeedItemPollAnswer>

type DeviantArtFeedItemPoll = {
    question: string
    total_votes: int
    answers: DeviantArtFeedItemPollAnswer[]
} with
    interface IBclDeviantArtFeedItemPoll with
        member this.Answers = this.answers |> Seq.map (fun o -> o :> IBclDeviantArtFeedItemPollAnswer)
        member this.Question = this.question
        member this.TotalVotes = this.total_votes

type IBclDeviantArtFeedItem =
    abstract member Ts: DateTimeOffset
    abstract member Type: string
    abstract member ByUser: IBclDeviantArtUser
    abstract member Deviations: seq<IBclDeviation>
    abstract member Bucketid: Nullable<Guid>
    abstract member BucketTotal: Nullable<int>
    abstract member Status: IBclDeviantArtStatus
    abstract member Comment: IBclDeviantArtComment
    abstract member CommentParent: IBclDeviantArtComment
    abstract member CommentDeviation: IBclDeviation
    abstract member CommentProfile: IBclDeviantArtProfile
    abstract member CritiqueText: string
    abstract member Collection: IBclDeviantArtFeedItemCollection
    abstract member Formerly: string
    abstract member AddedCount: Nullable<int>
    abstract member Poll: IBclDeviantArtFeedItemPoll

type DeviantArtFeedItem = {
    ts: DateTimeOffset
    ``type``: string
    by_user: DeviantArtUser
    deviations: Deviation[] option
    bucketid: Guid option
    bucket_total: int option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    comment_parent: DeviantArtComment option
    comment_deviation: Deviation option
    comment_profile: DeviantArtProfile option
    critique_text: string option
    collection: DeviantArtFeedItemCollection option
    formerly: string option
    added_count: int option
    poll: DeviantArtFeedItemPoll option
} with
    interface IBclDeviantArtFeedItem with
        member this.AddedCount = this.added_count |> Option.toNullable
        member this.BucketTotal = this.bucket_total |> Option.toNullable
        member this.Bucketid = this.bucketid |> Option.toNullable
        member this.ByUser = this.by_user :> IBclDeviantArtUser
        member this.Collection = this.collection |> Option.map (fun o -> o :> IBclDeviantArtFeedItemCollection) |> Option.toObj
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