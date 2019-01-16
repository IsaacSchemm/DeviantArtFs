namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtWatchInfo =
    abstract member Friend: bool
    abstract member Deviations: bool
    abstract member Journals: bool
    abstract member ForumThreads: bool
    abstract member Critiques: bool
    abstract member Scraps: bool
    abstract member Activity: bool
    abstract member Collections: bool

type DeviantArtWatchInfo = {
    friend: bool
    deviations: bool
    journals: bool
    forum_threads: bool
    critiques: bool
    scraps: bool
    activity: bool
    collections: bool
} with
    interface IBclDeviantArtWatchInfo with
        member this.Activity = this.activity
        member this.Collections = this.collections
        member this.Critiques = this.critiques
        member this.Deviations = this.deviations
        member this.ForumThreads = this.forum_threads
        member this.Friend = this.friend
        member this.Journals = this.journals
        member this.Scraps = this.scraps

type IDeviantArtRelationshipRecord =
    abstract member User: IBclDeviantArtUser
    abstract member IsWatching: bool
    abstract member Watch: IBclDeviantArtWatchInfo

type IBclDeviantArtFriendRecord =
    inherit IDeviantArtRelationshipRecord
    abstract member WatchesYou: bool

type DeviantArtFriendRecord = {
    user: DeviantArtUser
    is_watching: bool
    watches_you: bool
    watch: DeviantArtWatchInfo
} with
    static member Parse json = Json.deserialize<DeviantArtFriendRecord> json
    interface IBclDeviantArtFriendRecord with
        member this.User = this.user :> IBclDeviantArtUser
        member this.IsWatching = this.is_watching
        member this.WatchesYou = this.watches_you
        member this.Watch = this.watch :> IBclDeviantArtWatchInfo

type IBclDeviantArtWatcherRecord =
    inherit IDeviantArtRelationshipRecord
    abstract member Lastvisit: Nullable<DateTimeOffset>

type DeviantArtWatcherRecord = {
    user: DeviantArtUser
    is_watching: bool
    lastvisit: DateTimeOffset option
    watch: DeviantArtWatchInfo
} with
    static member Parse json = Json.deserialize<DeviantArtWatcherRecord> json
    interface IBclDeviantArtWatcherRecord with
        member this.User = this.user :> IBclDeviantArtUser
        member this.IsWatching = this.is_watching
        member this.Lastvisit = this.lastvisit |> Option.toNullable
        member this.Watch = this.watch :> IBclDeviantArtWatchInfo