namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash
open System

type IDeltaEntry =
    abstract member Itemid: int64 option
    abstract member Stackid: int64 option
    abstract member Metadata: StackResponse.Root option
    abstract member Position: int option

type WrappedDeltaEntry(entry: DeltaResultEntry) =
    interface IDeltaEntry with
        member __.Itemid = entry.Itemid
        member __.Stackid = entry.Stackid
        member __.Metadata = entry.Metadata
        member __.Position = entry.Position

type SerializedDeltaEntry =
    {
        Itemid: Nullable<int64>
        Stackid: int64
        Metadata: StackResponse.Root
        Position: int
    }
    interface IDeltaEntry with
        member this.Itemid = this.Itemid |> Option.ofNullable
        member this.Stackid = Some this.Stackid
        member this.Metadata = Some this.Metadata
        member this.Position = Some this.Position