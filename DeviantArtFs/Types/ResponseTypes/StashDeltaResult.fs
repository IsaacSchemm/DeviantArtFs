namespace DeviantArtFs

open System
open FSharp.Json

type StashDeltaResult = {
    cursor: string
    has_more: bool
    next_offset: int option
    reset: bool
    entries: StashDeltaEntry list
} with
    static member Parse json = Json.deserialize<StashDeltaResult> json
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    interface IResultPage<int, StashDeltaEntry> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.entries |> Seq.ofList