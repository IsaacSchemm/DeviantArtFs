namespace DeviantArtFs.ResponseTypes

open System

type WatchInfo = {
    friend: bool
    deviations: bool
    journals: bool
    forum_threads: bool
    critiques: bool
    scraps: bool
    activity: bool
    collections: bool
}

type FriendRecord = {
    user: User
    is_watching: bool
    watches_you: bool
    watch: WatchInfo
}

type WatcherRecord = {
    user: User
    is_watching: bool
    lastvisit: DateTimeOffset option
    watch: WatchInfo
}