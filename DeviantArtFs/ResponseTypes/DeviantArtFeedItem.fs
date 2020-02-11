namespace DeviantArtFs

open System

type DeviantArtFeedItemCollection = {
    folderid: Guid
    name: string
    url: string
    size: int option
}

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
}