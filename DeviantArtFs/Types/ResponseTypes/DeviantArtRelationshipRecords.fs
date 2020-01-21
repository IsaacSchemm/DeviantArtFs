namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtWatchInfo = {
    friend: bool
    deviations: bool
    journals: bool
    forum_threads: bool
    critiques: bool
    scraps: bool
    activity: bool
    collections: bool
}

type DeviantArtFriendRecord = {
    user: DeviantArtUser
    is_watching: bool
    watches_you: bool
    watch: DeviantArtWatchInfo
}

type DeviantArtWatcherRecord = {
    user: DeviantArtUser
    is_watching: bool
    lastvisit: DateTimeOffset option
    watch: DeviantArtWatchInfo
} with
    static member Parse json = Json.deserialize<DeviantArtWatcherRecord> json