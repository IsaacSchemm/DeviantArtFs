namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtCommentSiblingsContext =
    abstract member Parent: IBclDeviantArtComment
    abstract member ItemProfile: IBclDeviantArtUser
    abstract member ItemDeviation: IBclDeviation
    abstract member ItemStatus: IBclDeviantArtStatus

type DeviantArtCommentSiblingsContext = {
    parent: DeviantArtComment option
    item_profile: DeviantArtUser option
    item_deviation: Deviation option
    item_status: DeviantArtStatus option
} with
    interface IBclDeviantArtCommentSiblingsContext with
        member this.Parent = this.parent |> Option.map (fun o -> o :> IBclDeviantArtComment) |> Option.toObj
        member this.ItemProfile = this.item_profile |> Option.map (fun o -> o :> IBclDeviantArtUser) |> Option.toObj
        member this.ItemDeviation = this.item_deviation |> Option.map (fun o -> o :> IBclDeviation) |> Option.toObj
        member this.ItemStatus = this.item_status |> Option.map (fun o -> o :> IBclDeviantArtStatus) |> Option.toObj

type IBclDeviantArtCommentSiblingsPagedResult =
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: bool
    abstract member PrevOffset: Nullable<int>
    abstract member Thread: seq<IBclDeviantArtComment>
    abstract member Context: IBclDeviantArtCommentSiblingsContext

type DeviantArtCommentSiblingsPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    thread: DeviantArtComment list
    context: DeviantArtCommentSiblingsContext
} with
    static member Parse (json: string) =
        json.Replace(""""context": list""", """"context":{}""")
        |> Json.deserialize<DeviantArtCommentSiblingsPagedResult>
    interface IBclDeviantArtCommentSiblingsPagedResult with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.HasLess = this.has_less
        member this.PrevOffset = this.prev_offset |> Option.toNullable
        member this.Thread = this.thread |> Seq.map (fun c -> c :> IBclDeviantArtComment)
        member this.Context = this.context :> IBclDeviantArtCommentSiblingsContext
    interface IResultPage<int, DeviantArtComment> with
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.HasMore = this.has_more
        member this.Items = this.thread |> Seq.ofList