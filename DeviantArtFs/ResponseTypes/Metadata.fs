namespace DeviantArtFs.ResponseTypes

open System

type Metadata = {
    deviationid: Guid
    printid: Guid option
    author: User
    is_watching: bool
    title: string
    description: string
    license: string
    allows_comments: bool
    tags: Tag list
    is_favourited: bool
    is_mature: bool
    submission: MetadataSubmission option
    stats: MetadataStats option
    camera: Map<string, string> option
    collections: CollectionFolder list option
    can_post_comment: bool
}

type MetadataResponse = {
    metadata: Metadata list
}