namespace DeviantArtFs

open System

type DeviantArtFeedItemCollection = {
    folderid: Guid
    name: string
    url: string
    size: int option
} with
    member this.GetSize() = OptUtils.intDefault this.size 

type DeviantArtFeedItemPollAnswer = {
    answer: string
    votes: int
}

type DeviantArtFeedItemPoll = {
    question: string
    total_votes: int
    answers: DeviantArtFeedItemPollAnswer list
}

type DeviantArtFeedItem = {
    ts: DateTimeOffset
    ``type``: string
    by_user: DeviantArtUser
    deviations: Deviation list option
    bucketid: Guid option
    bucket_total: int option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    comment_parent: DeviantArtComment option
    comment_deviation: Deviation option
    comment_profile: DeviantArtProfile option
    comment_status: DeviantArtStatus option
    critique_text: string option
    collection: DeviantArtFeedItemCollection option
    formerly: string option
    added_count: int option
    poll: DeviantArtFeedItemPoll option
} with
    member this.GetAddedCount() = OptUtils.intDefault this.added_count
    member this.GetBucketTotal() = OptUtils.intDefault this.bucket_total
    member this.GetBucketId() = OptUtils.guidDefault this.bucketid
    member this.GetDeviations() = OptUtils.listDefault this.deviations
    member this.GetCollection() = OptUtils.recordDefault this.collection
    member this.GetComment() = OptUtils.recordDefault this.comment
    member this.GetCommentDeviation() = OptUtils.recordDefault this.comment_deviation
    member this.GetCommentParent() = OptUtils.recordDefault this.comment_parent
    member this.GetCommentProfile() = OptUtils.recordDefault this.comment_profile
    member this.GetCommentStatus() = OptUtils.recordDefault this.comment_status
    member this.GetCritique() = OptUtils.recordDefault this.critique_text
    member this.GetFormerly() = OptUtils.recordDefault this.formerly
    member this.GetPoll() = OptUtils.recordDefault this.poll
    member this.GetStatus() = OptUtils.recordDefault this.status