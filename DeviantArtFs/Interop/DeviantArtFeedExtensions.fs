namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtFeedExtensions =
    [<Extension>]
    let GetAddedCount (this: DeviantArtFeedItem) =
        OptUtils.intDefault this.added_count

    [<Extension>]
    let GetBucketTotal (this: DeviantArtFeedItem) =
        OptUtils.intDefault this.bucket_total

    [<Extension>]
    let GetBucketId (this: DeviantArtFeedItem) =
        OptUtils.guidDefault this.bucketid

    [<Extension>]
    let GetDeviations (this: DeviantArtFeedItem) =
        OptUtils.listDefault this.deviations

    [<Extension>]
    let GetCollection (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.collection

    [<Extension>]
    let GetComment (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.comment

    [<Extension>]
    let GetCommentDeviation (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.comment_deviation

    [<Extension>]
    let GetCommentParent (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.comment_parent

    [<Extension>]
    let GetCommentProfile (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.comment_profile

    [<Extension>]
    let GetCommentStatus (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.comment_status

    [<Extension>]
    let GetCritique (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.critique_text

    [<Extension>]
    let GetFormerly (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.formerly

    [<Extension>]
    let GetPoll (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.poll

    [<Extension>]
    let GetStatus (this: DeviantArtFeedItem) =
        OptUtils.recordDefault this.status