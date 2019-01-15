namespace DeviantArtFs

open System
open FSharp.Data

type internal FriendsElement = JsonProvider<"""{
    "user": {
        "userid": "EDCB4A55-BAE8-C146-B390-5118088A0CF5",
        "username": "muteor",
        "usericon": "https://a.deviantart.net/avatars/m/u/muteor.png?2",
        "type": "regular"
    },
    "is_watching": true,
    "watches_you": false,
    "watch": {
        "friend": true,
        "deviations": false,
        "journals": true,
        "forum_threads": true,
        "critiques": false,
        "scraps": false,
        "activity": false,
        "collections": false
    }
}""">

type internal WatchersElement = JsonProvider<"""[
{
    "user": {
        "userid": "EDCB4A55-BAE8-C146-B390-5118088A0CF5",
        "username": "muteor",
        "usericon": "https://a.deviantart.net/avatars/m/u/muteor.png?2",
        "type": "regular"
    },
    "is_watching": true,
    "lastvisit": "2014-10-13T22:34:16-0700",
    "watch": {
        "friend": true,
        "deviations": true,
        "journals": true,
        "forum_threads": true,
        "critiques": true,
        "scraps": false,
        "activity": true,
        "collections": true
    }
},
{
    "user": {
        "userid": "EDCB4A55-BAE8-C146-B390-5118088A0CF5",
        "username": "muteor",
        "usericon": "https://a.deviantart.net/avatars/m/u/muteor.png?2",
        "type": "regular"
    },
    "is_watching": true,
    "lastvisit": null,
    "watch": {
        "friend": true,
        "deviations": true,
        "journals": true,
        "forum_threads": true,
        "critiques": true,
        "scraps": false,
        "activity": true,
        "collections": true
    }
}
]""", SampleIsList=true>

type IDeviantArtWatchInfo =
    abstract member Friend: bool
    abstract member Deviations: bool
    abstract member Journals: bool
    abstract member ForumThreads: bool
    abstract member Critiques: bool
    abstract member Scraps: bool
    abstract member Activity: bool
    abstract member Collections: bool

type IDeviantArtRelationshipRecord =
    abstract member User: IBclDeviantArtUser
    abstract member IsWatching: bool
    abstract member Watch: IDeviantArtWatchInfo

type IBclDeviantArtFriendRecord =
    inherit IDeviantArtRelationshipRecord
    abstract member WatchesYou: bool

type DeviantArtFriendRecord(original: FriendsElement.Root) =
    member __.User = {
        new IBclDeviantArtUser with
            member __.Userid = original.User.Userid
            member __.Username = original.User.Username
            member __.Usericon = original.User.Usericon
            member __.Type = original.User.Type
    }
    member __.IsWatching = original.IsWatching
    member __.WatchesYou = original.WatchesYou
    member __.Watch = {
        new IDeviantArtWatchInfo with
            member __.Friend = original.Watch.Friend
            member __.Deviations = original.Watch.Deviations
            member __.Journals = original.Watch.Journals
            member __.ForumThreads = original.Watch.ForumThreads
            member __.Critiques = original.Watch.Critiques
            member __.Scraps = original.Watch.Scraps
            member __.Activity = original.Watch.Activity
            member __.Collections = original.Watch.Collections
    }

    interface IBclDeviantArtFriendRecord with
        member this.User = this.User
        member this.IsWatching = this.IsWatching
        member this.WatchesYou = this.WatchesYou
        member this.Watch = this.Watch

type IBclDeviantArtWatcherRecord =
    inherit IDeviantArtRelationshipRecord
    abstract member Lastvisit: Nullable<DateTimeOffset>

type DeviantArtWatcherRecord(original: WatchersElement.Root) =
    member __.User = {
        new IBclDeviantArtUser with
            member __.Userid = original.User.Userid
            member __.Username = original.User.Username
            member __.Usericon = original.User.Usericon
            member __.Type = original.User.Type
    }
    member __.IsWatching = original.IsWatching
    member __.Lastvisit = original.Lastvisit
    member __.Watch = {
        new IDeviantArtWatchInfo with
            member __.Friend = original.Watch.Friend
            member __.Deviations = original.Watch.Deviations
            member __.Journals = original.Watch.Journals
            member __.ForumThreads = original.Watch.ForumThreads
            member __.Critiques = original.Watch.Critiques
            member __.Scraps = original.Watch.Scraps
            member __.Activity = original.Watch.Activity
            member __.Collections = original.Watch.Collections
    }

    interface IBclDeviantArtWatcherRecord with
        member this.User = this.User
        member this.IsWatching = this.IsWatching
        member this.Lastvisit = this.Lastvisit |> Option.toNullable
        member this.Watch = this.Watch