namespace DeviantArtFs

open System

type IBclDeltaResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member Reset: bool
    abstract member Entries: seq<IBclDeltaEntry>

type DeltaResult(original: DeltaResponse.Root) =
    member __.Original = original

    member __.Cursor = original.Cursor
    member __.HasMore = original.HasMore
    member __.NextOffset = original.NextOffset
    member __.Reset = original.Reset
    member __.Entries = original.Entries |> Seq.map DeltaEntry

    interface IBclDeltaResult with
        member this.Cursor = this.Cursor
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.Reset = this.Reset
        member this.Entries = this.Entries |> Seq.map (fun e -> e :> IBclDeltaEntry)