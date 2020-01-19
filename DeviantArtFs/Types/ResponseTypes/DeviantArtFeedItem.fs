namespace DeviantArtFs

open System

type DeviantArtFeedItemCollection = {
    folderid: Guid
    name: string
    url: string
    size: int option
} with
    member this.GetSize() = OptUtils.toNullable this.size 

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
    member this.GetAddedCount() = OptUtils.toNullable this.added_count
    member this.GetBucketTotal() = OptUtils.toNullable this.bucket_total
    member this.GetBucketId() = OptUtils.toNullable this.bucketid
    member this.GetDeviations() = OptUtils.listDefault this.deviations
    member this.GetCollections() = OptUtils.toSeq this.collection
    member this.GetComments() = OptUtils.toSeq this.comment
    member this.GetCommentDeviations() = OptUtils.toSeq this.comment_deviation
    member this.GetCommentParents() = OptUtils.toSeq this.comment_parent
    member this.GetCommentProfiles() = OptUtils.toSeq this.comment_profile
    member this.GetCommentStatuses() = OptUtils.toSeq this.comment_status
    member this.GetCritiques() = OptUtils.toSeq this.critique_text
    member this.GetFormerlys() = OptUtils.toSeq this.formerly
    member this.GetPolls() = OptUtils.toSeq this.poll
    member this.GetStatuses() = OptUtils.toSeq this.status