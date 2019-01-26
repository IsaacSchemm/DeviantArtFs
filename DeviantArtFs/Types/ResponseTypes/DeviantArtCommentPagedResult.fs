namespace DeviantArtFs

open System
open FSharp.Json

type IBclDeviantArtCommentPagedResult =
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: bool
    abstract member PrevOffset: Nullable<int>
    abstract member Total: Nullable<int>
    abstract member Thread: seq<IBclDeviantArtComment>

type DeviantArtCommentPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    total: int option
    thread: DeviantArtComment[]
} with
    static member Parse json = Json.deserialize<DeviantArtCommentPagedResult> json
    interface IBclDeviantArtCommentPagedResult with
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.HasLess = this.has_less
        member this.PrevOffset = this.prev_offset |> Option.toNullable
        member this.Total = this.total |> Option.toNullable
        member this.Thread = this.thread |> Seq.map (fun c -> c :> IBclDeviantArtComment)
    interface IResultPage<int, DeviantArtComment> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.thread |> Seq.ofArray