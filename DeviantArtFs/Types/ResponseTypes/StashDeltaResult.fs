namespace DeviantArtFs

open System
open FSharp.Json

type IBclStashDeltaResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member Reset: bool
    abstract member Entries: seq<IBclStashDeltaEntry>

type StashDeltaResult = {
    cursor: string
    has_more: bool
    next_offset: int option
    reset: bool
    entries: StashDeltaEntry[]
} with
    static member Parse json = Json.deserialize<StashDeltaResult> json
    interface IBclStashDeltaResult with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.NextOffset = this.next_offset |> Option.toNullable
        member this.Reset = this.reset
        member this.Entries = this.entries |> Seq.map (fun e -> e :> IBclStashDeltaEntry)
    interface IResultPage<int, StashDeltaEntry> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.entries |> Seq.ofArray