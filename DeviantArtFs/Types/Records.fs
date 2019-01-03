namespace DeviantArtFs

open System

type UserResult = {
    Userid: Guid
    Username: string
    Usericon: string
    Type: string
} with
    interface IDeviantArtUser with
        member this.Userid = this.Userid
        member this.Username = this.Username
        member this.Usericon = this.Usericon
        member this.Type = this.Type

type WatchInfo = {
    Friend: bool
    Deviations: bool
    Journals: bool
    ForumThreads: bool
    Critiques: bool
    Scraps: bool
    Activity: bool
    Collections: bool
}