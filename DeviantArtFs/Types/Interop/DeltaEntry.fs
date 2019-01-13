namespace DeviantArtFs.Interop

open DeviantArtFs

type DeltaEntry(original: IBclDeltaEntry) =
    let metadata =
        original.MetadataJson
        |> Option.ofObj
        |> Option.map StashMetadataResponse.Parse
        |> Option.map StashMetadata

    member __.Original = original

    member __.Itemid = original.Itemid
    member __.Stackid = original.Stackid
    member __.MetadataJson = original.MetadataJson
    member __.Position = original.Position

    member __.Metadata = metadata |> Option.map (fun m -> m :> IBclStashMetadata) |> Option.toObj

    interface IBclDeltaEntry with
        member this.Itemid = this.Itemid
        member this.Stackid = this.Stackid
        member this.MetadataJson = this.MetadataJson
        member this.Position = this.Position

type IDeltaResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member NextOffset: System.Nullable<int>
    abstract member Reset: bool
    abstract member Entries: seq<DeltaEntry>