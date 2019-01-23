namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtFeedNotification =
    abstract member Ts: DateTimeOffset
    abstract member Type: string
    abstract member ByUser: IBclDeviantArtUser
    abstract member Deviations: seq<IBclDeviation>
    abstract member Comment: IBclDeviantArtComment
    abstract member CommentParent: IBclDeviantArtComment
    abstract member CommentDeviation: IBclDeviation
    abstract member CommentProfile: IBclDeviantArtProfile
    abstract member Status: IBclDeviantArtStatus
    abstract member CommentStatus: IBclDeviantArtStatus

type DeviantArtFeedNotification = {
    ts: DateTimeOffset
    ``type``: string
    by_user: DeviantArtUser
    deviations: Deviation[] option
    comment: DeviantArtComment option
    comment_parent: DeviantArtComment option
    comment_deviation: Deviation option
    comment_profile: DeviantArtProfile option
    status: DeviantArtStatus option
    comment_status: DeviantArtStatus option
} with
    static member Parse json = Json.deserialize<DeviantArtFeedNotification> json
    interface IBclDeviantArtFeedNotification with
        member this.ByUser = this.by_user :> IBclDeviantArtUser
        member this.Comment = this.comment |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.CommentDeviation = this.comment_deviation |> Option.map (fun o -> o :> IBclDeviation) |> Option.toObj
        member this.CommentParent = this.comment_parent |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.CommentProfile = this.comment_profile |> Option.map (fun o -> o :> IBclDeviantArtProfile) |> Option.toObj
        member this.CommentStatus = this.comment_status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj
        member this.Deviations = this.deviations |> Option.map Seq.ofArray |> Option.defaultValue Seq.empty |> Seq.map (fun d -> d :> IBclDeviation)
        member this.Status = this.status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj
        member this.Ts = this.ts
        member this.Type = this.``type``